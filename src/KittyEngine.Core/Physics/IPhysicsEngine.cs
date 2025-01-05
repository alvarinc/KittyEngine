
using KittyEngine.Core.State;

namespace KittyEngine.Core.Physics
{
    public interface IPhysicsEngine
    {
        bool UpdatePhysics(GameState gameState, double deltaTimeInMilliseconds);
    }
}
