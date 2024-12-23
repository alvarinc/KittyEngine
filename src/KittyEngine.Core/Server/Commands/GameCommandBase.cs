using KittyEngine.Core.Server.Model;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal abstract class GameCommandBase : IGameCommand
    {
        public abstract bool ValidateParameters(GameCommandInput input);
        public abstract GameCommandResult Execute(GameState gameState, Player player);
    }
}
