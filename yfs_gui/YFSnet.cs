using System.Net.Sockets;
using System.Text;
using System.Net;

namespace yfs_net;

public class YFSnet
{
    public Socket createClient(string ip, int port)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        Socket __socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        __socket.Connect(ipPoint);

        return __socket;
    }

    public void sendData(Socket __socket, string data)
    {
        try
        {
            byte[] buff = Encoding.UTF8.GetBytes(data);
            byte[] buffLength = { (byte)buff.Length };
            __socket.Send(buffLength);
            __socket.Send(buff);
        }
        catch (SocketException)
        {
            MessageBox.Show(
                "Сервер отключился",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    public byte[] getData(Socket __socket)
    {
        try
        {
            byte[] getBuffLength = new byte[1];
            __socket.Receive(getBuffLength);
            byte[] buff = new byte[getBuffLength[0]];
            __socket.Receive(buff);

            return buff;
        }
        catch (SocketException)
        {
            return Encoding.UTF8.GetBytes("closeconn");
        }
    }
}
