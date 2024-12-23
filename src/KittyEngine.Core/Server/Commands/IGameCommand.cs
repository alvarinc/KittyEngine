using KittyEngine.Core.Server.Model;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal interface IGameCommand
    {
        bool ValidateParameters(GameCommandInput cmd);
        GameCommandResult Execute(GameState gameState, Player player);
    }
}
