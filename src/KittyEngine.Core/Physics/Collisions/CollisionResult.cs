using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionResult
    {
        public bool HasCollision { get; set; }
        public bool AlternateMoveComputed { get; set; }
        public Vector3D[] AlternateMoveDirections { get; set; }
        public double AlternateMoveSpeedPercentage { get; set; }
    }
}
