using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Physics.Collisions.BVH;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionDetectionParameters
    {
        public BVHTreeNode<LayeredModel3D> BvhTree { get; set; }

        public IMovableBody MovableBody { get; set; }
        public Vector3D MoveDirection { get; set; }
    }
}
