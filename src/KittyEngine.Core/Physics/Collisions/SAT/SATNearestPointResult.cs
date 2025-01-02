
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.SAT
{
    public class SATNearestPointResult
    {
        public double NearestDistance { get; set; } = double.PositiveInfinity;
        public Vector3D NearestMove { get; set; }
    }
}
