using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    public interface IInputHandler
    {
        bool IsEnabled { get; }
        void Disable();
        void Enable();
        void Reset();

        void RegisterEvents(object gameHost);

        List<GameCommandInput> HandleEvents(GameState gameState, string playerId);
    }
}
