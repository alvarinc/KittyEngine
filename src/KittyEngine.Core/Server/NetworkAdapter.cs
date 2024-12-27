using LiteNetLib.Utils;
using LiteNetLib;
using Newtonsoft.Json;
using KittyEngine.Core.Services.Network;
using System.Text;

namespace KittyEngine.Core.Server
{
    public class NetworkAdapter
    {
        private NetManager _server;
        private LargeMessageSender _largeMessageSender;

        public NetworkAdapter(NetManager server)
        {
            _server = server;
            _largeMessageSender = new LargeMessageSender();
        }

        public IEnumerable<NetPeer> GetConnectedPeers()
        {
            return _server.ConnectedPeerList;
        }

        public void SendMessage(NetPeer peer, GameCommandInput cmd)
        {
            var writer = new NetDataWriter();
            var json = JsonConvert.SerializeObject(cmd);
            Console.WriteLine($"Sending message: {json}");

            var data = Encoding.UTF8.GetBytes(json);
            _largeMessageSender.SendLargeMessage(peer, data);
        }

        public void HandlePeersEvents()
        {
            _server.PollEvents();
        }
    }
}
