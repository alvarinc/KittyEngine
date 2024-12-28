using KittyEngine.Core.Server.Model;

namespace KittyEngine.Core.State
{
    public class ClientState
    {
        public ClientState()
        {
            Mode = ClientMode.InGame;
            ConnectedUser = null;
            GameState = new GameState();
            Graphics = new GraphicState();
            ClientWindow = new ClientWindowState();
        }

        public ClientMode Mode { get; set; }
        public Player ConnectedUser { get; set; }
        public GameState GameState { get; set; }
        public GraphicState Graphics { get; set; }

        public ClientWindowState ClientWindow { get; set; }
    }
}
