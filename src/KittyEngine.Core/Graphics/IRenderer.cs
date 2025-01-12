using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics
{
    public interface IRenderer
    {
        void RegisterOutput(IGameHost host);

        void RenderFrame(GameState _gameState, string playerId);
    }
}
