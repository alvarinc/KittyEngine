using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionDetail
    {
        public LayeredModel3D CollidedObject { get; set; }
        public List<Triangle3D> CollidedTriangles { get; set; }
    }
}
