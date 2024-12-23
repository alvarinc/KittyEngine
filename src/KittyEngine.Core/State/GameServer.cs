
namespace KittyEngine.Core.State
{
    public class GameServer
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public GameServer(string address, int port)
        {
            Address = address;
            Port = port;
        }
    }
}
