using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using yfs_gui;
using yfs_security;

namespace yfs_io;

public class YFSio
{
    YFSsec sec = new();

    public void uploadFile(Socket __socket, string file, ProgressBar progressBar,
        Label label, string? folder = null,
        bool encryptFile = false)
    {
        string path;
        if (folder == null)
            path = file;
        else
            path = $"{folder}/{file}";

        string keyFile = "";
        if (encryptFile)
        {
            label.Location = new Point(309, 350);
            label.BackColor = Color.FromArgb(230, 230, 230);
            label.Visible = true;
            label.Text = "Файл шифруется...";
            label.Refresh();

            keyFile = sec.encryptFile(path);
            file += ".enc";
            path += ".enc";

            label.Visible = false;
            label.Refresh();
        }

        BinaryReader b = new(File.Open(path, FileMode.Open));

        byte[] getFileName = Encoding.UTF8.GetBytes(file);
        byte[] getFileName_arrLength = { (byte)getFileName.Length };
        byte[] getFileLength = BitConverter.GetBytes(b.BaseStream.Length);
        byte[] getFileLength_arrSize = { (byte)getFileLength.Length };
        b.Close();
        byte[] getFileChecksum = Encoding.UTF8.GetBytes(sec.checksumFileSHA256(path));

        __socket.Send(getFileName_arrLength);
        __socket.Send(getFileName);
        __socket.Send(getFileLength_arrSize);
        __socket.Send(getFileLength);
        __socket.Send(getFileChecksum);

        BinaryReader br = new(File.Open(path, FileMode.Open));
        br.BaseStream.Position = 0;

        long c = 1;
        progressBar.Maximum = (int)(br.BaseStream.Length / (1024 * 58));

        while (br.BaseStream.Position != br.BaseStream.Length)
        {
            byte[] readByte = { br.ReadByte() };
            __socket.Send(readByte);
            if (br.BaseStream.Position == c)
            {
                //Console.WriteLine($"[UPLOAD] {Path.GetFileName(path)} [{br.BaseStream.Position / 1024} кб / {br.BaseStream.Length / 1024} кб]");
                progressBar.PerformStep();
                c += 1024 * 58;
            }
        }
        br.Close();

        byte[] uploadedFileChecksum = new byte[64];
        __socket.Receive(uploadedFileChecksum);

        if (Encoding.UTF8.GetString(getFileChecksum) != Encoding.UTF8.GetString(uploadedFileChecksum))
        {
            DialogResult result = MessageBox.Show($"""
                Хеши не совпадают. Возможно, файл при отправки на сервер был повреждён.

                SHA-256:
                    {Encoding.UTF8.GetString(getFileChecksum)} (отправленный файл) 
                                    !=
                    {Encoding.UTF8.GetString(uploadedFileChecksum)} (файл на сервере)
            
                Удалить файл?
                """,
                "",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error
            );

            if ( result == DialogResult.Yes )
            {
                byte[] sendAnswer = { 1 };
                __socket.Send(sendAnswer);
            }
            else
            {
                byte[] sendAnswer = { 0 };
                __socket.Send(sendAnswer);
            }
        }

        else
        {
            MessageBox.Show(
                "Загрузка завершена",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        if (encryptFile)
        {
            writeKeytableFile(__socket, keyFile, file.Remove(0, 1));
            File.Delete(path);
        }
        progressBar.Value = 0;
    }

    public void downloadFile(Socket __socket, string saveFolder, 
        ProgressBar progressBar, Label label)
    {
        byte[] getFileNameArrayLength = new byte[1];
        __socket.Receive(getFileNameArrayLength);

        if (getFileNameArrayLength[0] == 0)
        {
            MessageBox.Show(
                "Файл не найден",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return;
        }

        byte[] getFileName = new byte[getFileNameArrayLength[0]];
        __socket.Receive(getFileName);

        byte[] getFileArrayLength = new byte[1];
        __socket.Receive(getFileArrayLength);
        
        byte[] getFileLength = new byte[getFileArrayLength[0]];
        __socket.Receive(getFileLength);

        byte[] getFileChecksum = new byte[64];
        __socket.Receive(getFileChecksum);
        
        string savePath = $"{saveFolder}/{Path.GetFileName(Encoding.UTF8.GetString(getFileName))}";
        using BinaryWriter br = new(File.Open(savePath, FileMode.OpenOrCreate));
        long fLength = BitConverter.ToInt64(getFileLength);

        long c = 1;
        progressBar.Maximum = (int)(fLength / (1024*58));

        while (br.BaseStream.Position != fLength)
        {
            byte[] wr = new byte[1];
            __socket.Receive(wr);
            br.Write(wr[0]);

            if (br.BaseStream.Position == c)
            {
                // $"Скачивание {Encoding.UTF8.GetString(getFileName)} [{br.BaseStream.Position / 1024} кб / {fLength / 1024} кб]";
                progressBar.PerformStep();
                c += 1024 * 58;
            }
        }
        br.Close();

        string downloadedFileChecksum = sec.checksumFileSHA256(savePath);
        byte[] downloadedFileChecksum_bytes = Encoding.UTF8.GetBytes(downloadedFileChecksum);
        __socket.Send(downloadedFileChecksum_bytes);

        if (downloadedFileChecksum != Encoding.UTF8.GetString(getFileChecksum))
        {
            DialogResult result =  MessageBox.Show(
                $"""
                Хеши не совпадают. Возможно, файл при скачивании был повреждён.

                SHA-256:
                    {downloadedFileChecksum} (скачанный файл) 
                                !=
                    {Encoding.UTF8.GetString(getFileChecksum)} (файл на сервере)
                
                Удалить файл?
                """,
                "",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error
            );

            if (result == DialogResult.Yes)
            {
                File.Delete(savePath);
            }

            else { }
            
        }
        else
        {
            MessageBox.Show(
                "Скачивание завершено",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        if (savePath[^4..] == ".enc")
        {
            label.Location = new Point(288, 351);
            label.BackColor = Color.FromArgb(6, 176, 37);
            label.Visible = true;
            label.Text = "Файл расшифровывается...";
            label.Refresh();

            try
            {
                Dictionary<string, string> getKey = readStartupFile($"{((IPEndPoint)__socket.RemoteEndPoint).Address}.keytable");
                sec.decryptFile(savePath, getKey[Path.GetFileName(Encoding.UTF8.GetString(getFileName))]);
                File.Delete(savePath);
            }
            catch (KeyNotFoundException)
            {
                FormInputKey form = new();
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string getKey = form.inputKey.Text;

                    if (getKey != "")
                    {
                        sec.decryptFile(savePath, getKey);
                        File.Delete(savePath);
                    }
                }
            }
            label.Visible = false;
            label.Refresh();
        }
        
        progressBar.Value = 0;
    }

    public void writeKeytableFile(Socket __socket, string key, string file)
    {
        List<string> lines = new List<string>();
        IPEndPoint getIPAddres = __socket.RemoteEndPoint as IPEndPoint;
        string keyTableFile = $"{getIPAddres.Address}.keytable";

        if (File.Exists(keyTableFile))
        {
            string[] getLines = File.ReadAllLines(keyTableFile);
            lines = getLines.ToList();
            foreach (string line in lines.ToList())
            {
                if (line.Contains(Path.GetFileName(file)))
                {
                    int index = lines.IndexOf(line);
                    lines.RemoveAt(index);
                }
            }
            File.WriteAllText(keyTableFile, string.Empty);
        }

        using FileStream fs = new(keyTableFile, FileMode.Append);
        
        foreach (string s in lines.ToArray())
        {
            fs.Write(Encoding.UTF8.GetBytes(s+"\n"));
        }
        
        fs.Write(Encoding.UTF8.GetBytes($"{Path.GetFileName(file)}={key}\n"));  
    }

    public Dictionary<string, string>? readStartupFile(string startupFile)
    {
        Dictionary<string, string>? data = [];
        try
        {
            foreach (string line in File.ReadAllLines(startupFile))
            {
                string _key = "";
                string _value = "";

                foreach (char _char in line)
                {
                    if (_char != '=')
                        _key += _char;
                    else
                        break;
                }
                foreach (char _char in line.Reverse())
                {
                    if (_char != '=')
                        _value += _char;
                    else
                        break;
                }

                char[] _reverseStringValue = _value.ToCharArray();
                Array.Reverse(_reverseStringValue);

                data.Add(_key, new string(_reverseStringValue));
            }

            return data;
        }
        catch (FileNotFoundException)
        {
            data.Add("useStartupFile", "no");
            return data;
        }
    }
}
