using KittyEngine.Core.Graphics;

namespace KittyEngine.Core.Physics.Collisions.SAT
{
    public class SATCollisionResult
    {
        public bool HasIntersect => Intersections.Any();
        public List<Triangle3D> Intersections { get; set; } = new List<Triangle3D>();
    }
}
