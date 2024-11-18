using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text;
using yfs_keygen;

namespace yfs_security;

public class YFSsec
{
    private const byte BYTE255 = 0b11111111;

    public void sendAuthData(Socket __socket, string getIP, string getPass)
    {
        string login = getIP;
        string pass = getPass;
        string passHash = genSHA256(pass);

        byte[] getLoginLength = { (byte)login.Length };
        byte[] sendLogin = Encoding.UTF8.GetBytes(login);
        byte[] sendPassHash = Encoding.UTF8.GetBytes(passHash);

        __socket.Send(getLoginLength);
        __socket.Send(sendLogin);
        __socket.Send(sendPassHash);
    }

    public string checksumFileSHA256(string path)
    {
        using SHA256 sha256 = SHA256.Create();
        using FileStream fs = File.OpenRead(path);
        byte[] hashBytes = sha256.ComputeHash(fs);
        string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        return hash.ToLower();
    }

    private string genSHA256(string s)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
        string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        return hash.ToLower();
    }

    public string encryptFile(string openFile)
    {
        using BinaryReader readFile = new(File.Open(openFile, FileMode.Open));
        using BinaryWriter writeFile = new(File.Open(openFile+".enc", FileMode.Create));

        string key = new YFSkeygen()._keygen();
        byte[] _keyBytes = new YFSkeygen()._keyBytes(key);

        while (readFile.BaseStream.Position != readFile.BaseStream.Length)
        {
            byte wb = readFile.ReadByte();
            foreach (byte b in _keyBytes)
            {
                wb ^= b;
                wb = (byte)(BYTE255 - wb);
            }
            writeFile.Write(wb);
        }

        return key;
    }

    public void decryptFile(string openFile, string key)
    {
        using BinaryReader readFile = new(File.Open(openFile, FileMode.Open));
        using BinaryWriter writeFile = new(File.Open(openFile.Remove(openFile.Length-4), FileMode.Create));

        while (readFile.BaseStream.Position != readFile.BaseStream.Length)
        {
            byte wb = readFile.ReadByte();
            foreach (byte b in key.Reverse())
            {
                wb ^= b;
                wb = (byte)(BYTE255 - wb);
            }
            writeFile.Write(wb);
        }
    }
}
