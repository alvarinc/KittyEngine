using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    public interface IInputHandler
    {
        List<GameCommandInput> HandleEvents(GameState gameState, string playerId);
    }
}
