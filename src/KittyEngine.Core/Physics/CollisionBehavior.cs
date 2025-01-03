
namespace KittyEngine.Core.Physics
{
    [Flags]
    public enum CollisionBehavior
    {
        None = 0,
        CheckCollision = 1,
        CanWallSlide = 2,
        CanClimbStairs = 4,
    }
}
