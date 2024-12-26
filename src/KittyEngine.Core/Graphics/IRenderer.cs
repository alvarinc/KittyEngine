using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics
{
    public interface IRenderer
    {
        void RegisterGraphicOutput(IGameHost host);

        void Render(GameState _gameState, string playerId);
    }
}
