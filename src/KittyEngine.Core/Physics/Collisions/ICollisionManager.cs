
namespace KittyEngine.Core.Physics.Collisions
{
    public interface ICollisionManager
    {
        CollisionResult DetectCollisions(CollisionDetectionParameters parameters);

        StairClimbingResult ComputeStairClimbing(CollisionDetectionParameters parameters, CollisionResult collisionResult);

        WallSlidingResult ComputeWallSliding(CollisionDetectionParameters parameters, CollisionResult collisionResult);
    }
}
