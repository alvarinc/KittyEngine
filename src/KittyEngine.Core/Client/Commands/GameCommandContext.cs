using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Commands
{
    internal class GameCommandContext
    {
        public GameState GameState { get; set; }
        public string PlayerId { get; set; }
        public bool StateUpdated { get; set; }
    }
}
