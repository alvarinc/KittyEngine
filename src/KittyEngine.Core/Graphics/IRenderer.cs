using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics
{
    public interface IRenderer
    {
        void Render(GameState _gameState, string playerId);
    }
}
