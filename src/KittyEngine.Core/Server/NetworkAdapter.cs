using LiteNetLib.Utils;
using LiteNetLib;
using Newtonsoft.Json;

namespace KittyEngine.Core.Server
{
    public class NetworkAdapter
    {
        private NetManager _server;

        public NetworkAdapter(NetManager server)
        {
            _server = server;
        }

        public IEnumerable<NetPeer> GetConnectedPeers()
        {
            return _server.ConnectedPeerList;
        }

        public void SendMessage(NetPeer peer, GameCommandInput cmd)
        {
            var writer = new NetDataWriter();
            var json = JsonConvert.SerializeObject(cmd);
            writer.Put(json);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void HandlePeersEvents()
        {
            _server.PollEvents();
        }
    }
}
