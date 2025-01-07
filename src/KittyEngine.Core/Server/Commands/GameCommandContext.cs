using KittyEngine.Core.Server.Model;
using KittyEngine.Core.State;
using LiteNetLib;

namespace KittyEngine.Core.Server.Commands
{
    public class GameCommandContext
    {
        private NetworkAdapter _adapter;
        public GameState GameState { get; }
        public Player Player { get; }

        public GameCommandContext(NetworkAdapter adapter, GameState gameState, Player player)
        {
            _adapter = adapter;
            GameState = gameState;
            Player = player;
        }

        public void SendMessage(int peerId, GameCommandInput cmd)
        {
            var peer = _adapter.GetConnectedPeers().FirstOrDefault(x => x.Id == peerId);
            _adapter.SendMessage(peer, cmd);
        }

        public void SendMessage(NetPeer peer, GameCommandInput cmd)
        {
            _adapter.SendMessage(peer, cmd);
        }

        public IEnumerable<NetPeer> GetConnectedPeers()
        {
            return _adapter.GetConnectedPeers();
        }
    }
}
