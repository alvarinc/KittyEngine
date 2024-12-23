using LiteNetLib.Utils;
using LiteNetLib;
using Newtonsoft.Json;
using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client
{
    public class NetworkAdapter
    {
        private NetManager _client;

        public NetworkAdapter(NetManager client)
        {
            _client = client;
        }

        public void HandleServerEvents()
        {
            _client.PollEvents();
        }

        public void SendMessage(GameCommandInput input)
        {
            if (input != null && _client != null && _client.FirstPeer != null && _client.FirstPeer.ConnectionState == ConnectionState.Connected)
            {
                var writer = new NetDataWriter();
                var json = JsonConvert.SerializeObject(input);
                writer.Put(json);
                _client.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }
    }
}
