using KittyEngine.Core.Graphics;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.SAT
{
    public class SATNearestPointManager
    {
        public SATNearestPointResult CalculateNearestDistance(List<Triangle3D> a, List<Triangle3D> b, Vector3D axis)
        {
            var result = new SATNearestPointResult();

            // Ensure the axis is normalized
            axis.Normalize();

            // Get all vertices of A and B
            var verticesA = a.SelectMany(triangle => triangle.PositionsAsVectors).ToArray();
            var verticesB = b.SelectMany(triangle => triangle.PositionsAsVectors).ToArray();

            // Project vertices onto the axis
            var projectionsA = verticesA.Select(vertex => Vector3D.DotProduct(vertex, axis)).ToArray();
            var projectionsB = verticesB.Select(vertex => Vector3D.DotProduct(vertex, axis)).ToArray();

            // Find min and max projections for both sets
            var aMin = projectionsA.Min();
            var aMax = projectionsA.Max();
            var bMin = projectionsB.Min();
            var bMax = projectionsB.Max();

            // Calculate the distance based on the direction of the axis
            double distance;
            if (aMax < bMin) // A is completely below B along the axis
            {
                distance = bMin - aMax;
            }
            else if (bMax < aMin) // B is completely below A along the axis
            {
                distance = aMin - bMax;
            }
            else
            {
                // Overlap case: Distance is zero if projections overlap
                distance = 0;
            }

            //if (distance > .01)
            //{
            //    distance -= .01;
            //}
            //else
            //{
            //    distance = 0;
            //}

            // Update the result if this axis yields a smaller distance
            if (distance < result.NearestDistance)
            {
                result.NearestDistance = distance;
                result.NearestMove = axis * distance;
            }

            return result;
        }
    }
}
