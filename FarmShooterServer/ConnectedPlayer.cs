using System.Drawing;

namespace FarmShooterServer
{
    public class ConnectedPlayer
    {
        public Socket CurrentSocket;
        public byte[] Buffer = new byte[1024];

        public string Nickname;
        public Inventory Inventory = new Inventory();
        public Item SelectedItem;

        public PlayerState State = PlayerState.None;

        public Point Position;

        public ConnectedPlayer(Socket cur_socket, string name) 
        {
            CurrentSocket = cur_socket;
            Nickname = name;
        }
    }
}
