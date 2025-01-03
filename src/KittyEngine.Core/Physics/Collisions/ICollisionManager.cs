
namespace KittyEngine.Core.Physics.Collisions
{
    public interface ICollisionManager
    {
        CollisionResult DetectCollisions(CollisionDetectionParameters parameters);

        CollisionResult DetectCollisions(CollisionDetectionParameters parameters, CollisionBehavior behavior);
    }
}
