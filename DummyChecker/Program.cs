using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyChecker 
{
    class Program 
    {
        public static void Main() 
        {
            IPEndPoint IPPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20013);

            Socket MainSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            MainSocket.Connect(IPPoint);
            MainSocket.Send(Encoding.Unicode.GetBytes("Exselsior"));

            byte[] data = new byte[4];
            int bytes = MainSocket.Receive(data);

            StringBuilder sb = new StringBuilder();

            while (bytes > 0) 
            {
                sb.Append(Encoding.Unicode.GetString(data, 0, 4));
                bytes -= 4;
            }

            Console.WriteLine(sb.ToString());
        }
    }
}