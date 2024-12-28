using KittyEngine.Core.Server.Model;

namespace KittyEngine.Core.State
{
    public class ServerState
    {
        public ServerState()
        {
            ConnectedUsers = new Dictionary<int, Player>();
            GameState = new GameState();
        }

        public Dictionary<int, Player> ConnectedUsers { get; set; }
        public GameState GameState { get; set; }
    }
}
