
namespace KittyEngine.Core.Server.Commands
{
    internal struct GameCommandResult
    {
        public bool StateUpdated { get; set; }
        public bool PeerInitializated { get; set; }

        public GameCommandResult Append(GameCommandResult newResult)
        {
            return new GameCommandResult
            {
                StateUpdated = StateUpdated || newResult.StateUpdated,
                PeerInitializated = PeerInitializated || newResult.PeerInitializated
            };
        }
    }
}
