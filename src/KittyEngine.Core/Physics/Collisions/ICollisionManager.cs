
using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Physics.Collisions
{
    public interface ICollisionManager
    {
        CollisionResult DetectCollisions(CollisionDetectionParameters parameters);

        StairClimbingResult ComputeStairClimbing(CollisionDetectionParameters parameters, CollisionResult collisionResult, ILogger logger);

        WallSlidingResult ComputeWallSliding(CollisionDetectionParameters parameters, CollisionResult collisionResult);
    }
}
