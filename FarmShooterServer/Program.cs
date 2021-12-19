global using FarmShooter;
global using System.Net.Sockets;
global using System.Net;

using System.Threading.Tasks;
using System.Text;

namespace FarmShooterServer
{
    public class Program
    {
        public static List<ConnectedPlayer> ConnectedPlayers = new();

        public static IPEndPoint IPPoint = new IPEndPoint(IPAddress.Any, 20013);

        public static Socket MainSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        public static byte[,,] Map;

        public static void Main()
        {
            Random random = new Random();

            Map = new byte[3, 20, 20];
            for (int i = 0; i < 20; ++i)
            {
                for (int k = 0; k < 20; ++k) 
                {
                    Map[1, i, k] = (byte)random.Next(0, 4);
                }
            }

            MainSocket.Bind(IPPoint);

            MainSocket.Listen();

            MainSocket.BeginAccept(100, AsyncAccept, MainSocket);

            while (true) 
            {
                
            }
        }

        public static void AsyncAccept(IAsyncResult ar) 
        {
            Socket listener = (Socket)ar.AsyncState;

            byte[] buffer;
            int bytes_read;
            Socket connected = listener.EndAccept(out buffer, out bytes_read, ar);

            string result = Encoding.Unicode.GetString(buffer, 0, bytes_read);

            ConnectedPlayer player = new ConnectedPlayer(connected, result);
            ConnectedPlayers.Add(player);

            Console.WriteLine($"Игрок {0} подключился к серверу.", result);

            connected.Send(Encoding.Unicode.GetBytes($"{Map.GetLength(1)};{Map.GetLength(2)}"));

            connected.Send(Map.Cast<byte>().ToArray());

            connected.BeginReceive(player.Buffer, 0, 1024, 0, AsyncReceive, player);

            listener.BeginAccept(100, AsyncAccept, null);
        }

        public static void AsyncReceive(IAsyncResult ar) 
        {
            ConnectedPlayer player = (ConnectedPlayer)ar.AsyncState;

            int bytes_read = player.CurrentSocket.EndReceive(ar);

            string result = Encoding.Unicode.GetString(player.Buffer, 0, bytes_read);

            List<string> res = result.Split(';').ToList();
            res.RemoveAll(x => x == "" || x == " ");

            player.CurrentSocket.BeginReceive(player.Buffer, 0, 1024, 0, AsyncAccept, player);
        }
    }
}