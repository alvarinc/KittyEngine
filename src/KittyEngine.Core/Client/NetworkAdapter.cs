using LiteNetLib.Utils;
using LiteNetLib;
using Newtonsoft.Json;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.Network;
using System.Text;

namespace KittyEngine.Core.Client
{
    public class NetworkAdapter
    {
        private NetManager _client;
        private LargeMessageSender _largeMessageSender;

        public int PeerId => _client.FirstPeer.Id;

        public NetworkAdapter(NetManager client)
        {
            _client = client;
            _largeMessageSender = new LargeMessageSender();
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

                var data = Encoding.UTF8.GetBytes(json);
                _largeMessageSender.SendLargeMessage(_client.FirstPeer, data);
            }
        }
    }
}
