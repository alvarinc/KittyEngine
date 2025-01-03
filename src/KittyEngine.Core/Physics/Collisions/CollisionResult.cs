using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionResult
    {
        public bool HasCollision => Collisions.Count > 0;
        public List<CollisionDetail> Collisions { get; set; } = new List<CollisionDetail>();
        public bool NearestMoveComputed { get; set; }
        public Vector3D NearestMove { get; set; }
    }
}
