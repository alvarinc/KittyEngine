using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionResult
    {
        public bool HasCollision { get; set; }
        public bool NearestMoveComputed { get; set; }
        public Vector3D NearestMove { get; set; }
    }
}
