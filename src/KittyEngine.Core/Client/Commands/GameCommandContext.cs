using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Commands
{
    public class GameCommandContext
    {
        private NetworkAdapter _adapter;

        public ClientState State { get; set; }
        public bool StateUpdated { get; set; }

        public int PeerId => _adapter.PeerId;

        public GameCommandContext(NetworkAdapter adapter)
        {
            _adapter = adapter;
        }

        public void SendMessage(GameCommandInput input)
        {
            _adapter.SendMessage(input);
        }
    }
}
