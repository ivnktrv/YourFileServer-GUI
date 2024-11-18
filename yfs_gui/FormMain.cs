using System.Diagnostics.Eventing.Reader;
using System.Net.Sockets;
using System.Text;
using yfs_io;
using yfs_net;
using yfs_security;

namespace yfs_gui;

public partial class FormMain : Form
{
    Socket? _client;
    YFSio io = new();
    YFSnet net = new();
    YFSsec sec = new();

    string currentDir = "/";
    //List<string> filesList = [];

    public FormMain()
    {
        InitializeComponent();
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
        _client = net.createClient(inputIP.Text, int.Parse(inputPort.Text));
        sec.sendAuthData(_client, inputUser.Text, inputPass.Text);

        byte[] getAnswer = new byte[1];
        _client.Receive(getAnswer);

        if (getAnswer[0] == 1)
        {
            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = true;
        }
        else
        {
            MessageBox.Show(
                "Логин или пароль неверный. Возможно, сервер изменил данные для входа",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            _client.Close();
            return;
        }

        getFiles();

        windowFileInfo.Enabled = true;
        buttonDownload.Enabled = true;
        buttonUpload.Enabled = true;
        buttonDelete.Enabled = true;
    }

    private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (listFiles.SelectedItem.ToString().Contains("(DIR)"))
            {
                net.sendData(_client, "cd");

                string changeDir = listFiles.SelectedItem.ToString();

                net.sendData(_client, changeDir.Remove(0, 6));

                currentDir = changeDir;

                getFiles();
                return;
            }
            else if (listFiles.SelectedItem.ToString() == "...")
            {
                net.sendData(_client, "cd");
                net.sendData(_client, "..");

                currentDir = "/";

                getFiles();
                return;
            }
            net.sendData(_client, "fileinfo");
            net.sendData(_client, listFiles.SelectedItem.ToString());

            byte[] data = new byte[1024];
            _client.Receive(data);

            windowFileInfo.Text = Encoding.UTF8.GetString(data);
        }
        catch (NullReferenceException) { }
    }

    private void buttonDisconnect_Click(object sender, EventArgs e)
    {
        net.sendData(_client, "closeconn");
        _client.Close();
        listFiles.Items.Clear();
        windowFileInfo.Clear();
        buttonDisconnect.Enabled = false;
        buttonConnect.Enabled = true;
        buttonDownload.Enabled = false;
        buttonUpload.Enabled = false;
        buttonDelete.Enabled = false;
        windowFileInfo.Enabled = false;

    }

    private void getFiles()
    {
        net.sendData(_client, "list");
        listFiles.Items.Clear();
        listFiles.Items.Add("...");

        while (true)
        {
            byte[] buff = net.getData(_client);
            string getDirsAndFiles = Encoding.UTF8.GetString(buff);
            if (getDirsAndFiles == ":END_OF_LIST") break;
            listFiles.Items.Add(getDirsAndFiles);
        }
    }

    private void buttonDownload_Click(object sender, EventArgs e)
    {
        DialogResult result =  selectSavePath.ShowDialog();
        if (result == DialogResult.OK)
        {
            net.sendData(_client, "download");
            net.sendData(_client, listFiles.SelectedItem.ToString());
            progressBar.Value = 0;
            this.Enabled = false;
            io.downloadFile(_client, selectSavePath.SelectedPath, progressBar, labelEncryptingFile);
            this.Enabled = true;
        }
    }

    private void buttonUpload_Click(object sender, EventArgs e)
    {
        DialogResult result = selectUploadFile.ShowDialog();

        if (result == DialogResult.OK)
        {
            net.sendData(_client, "upload");

            string sendFile = selectUploadFile.FileName;
            progressBar.Value = 0;

            this.Enabled = false;
            if (MessageBox.Show(
                "Зашифровать файл перед отправкой?",
                "",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes)
                io.uploadFile(_client, sendFile, progressBar, labelEncryptingFile, encryptFile: true);
            else
                io.uploadFile(_client, sendFile, progressBar, labelEncryptingFile);
            this.Enabled = true;
        }
        getFiles();
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show(
            "Подтвердить действие?",
            "",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );
        if (result == DialogResult.Yes)
        {
            net.sendData(_client, "delete");
            net.sendData(_client, listFiles.SelectedItem.ToString());
            getFiles();
        }
    }

    private void inputIP_Click(object sender, EventArgs e)
    {
        inputIP.SelectAll();
    }

    private void inputPort_Click(object sender, EventArgs e)
    {
        inputPort.SelectAll();
    }

    private void inputUser_Click(object sender, EventArgs e)
    {
        inputUser.SelectAll();
    }

    private void inputPass_Click(object sender, EventArgs e)
    {
        inputPass.SelectAll();
    }
}