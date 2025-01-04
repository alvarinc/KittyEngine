
using KittyEngine.Core.State;

namespace KittyEngine.Core.Physics
{
    public interface IPhysicsEngine
    {
        void UpdatePhysics(GameState gameState, double deltaTimeInMilliseconds);
    }
}
