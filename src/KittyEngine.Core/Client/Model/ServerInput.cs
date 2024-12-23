
namespace KittyEngine.Core.Client.Model
{
    public class ServerInput
    {
        public string Address { get; }
        public int Port { get; }

        public ServerInput(string address, int port)
        {
            Address = address;
            Port = port;
        }
    }
}
