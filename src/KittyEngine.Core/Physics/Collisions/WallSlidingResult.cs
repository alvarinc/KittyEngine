using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public class WallSlidingResult
    {
        public bool CanWallSlide { get; set; }
        public Vector3D Direction { get; set; }
    }
}
