using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal interface IGameCommand
    {
        public bool ValidateParameters(GameCommandInput cmd);
        public GameCommandResult Execute(GameState gameState, Player player, int peerId);
    }
}
