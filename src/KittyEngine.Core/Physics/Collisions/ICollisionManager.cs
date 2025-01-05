
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public interface ICollisionManager
    {
        CollisionResult DetectCollisions(CollisionDetectionParameters parameters);

        StairClimbingResult ComputeStairClimbing(CollisionDetectionParameters parameters, CollisionResult collisionResult);

        WallSlidingResult ComputeWallSliding(CollisionDetectionParameters parameters, CollisionResult collisionResult);

        List<Point3D> GetCollidedPoints(CollisionDetectionParameters parameters, CollisionResult collisionResult);
    }
}
