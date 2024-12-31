using KittyEngine.Core.Graphics;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.SAT
{
    public class SATCollisionManager
    {
        private static readonly Vector3D VectorZero = new Vector3D(0, 0, 0);

        public SATCollisionResult DetectCollisionsWithInfos(List<Triangle3D> a, List<Triangle3D> b)
        {
            var results = new SATCollisionResult();

            var counter = 0;
            foreach (var triangleB in b)
            {
                counter++;
                var list = new List<Triangle3D>();
                list.Add(triangleB);
                if (DetectCollisions(a, list))
                {
                    results.Intersections.Add(triangleB);
                }
            }

            return results;
        }

        public bool DetectCollisions(List<Triangle3D> a, List<Triangle3D> b)
        {
            var verticesA = a.SelectMany(x => x.PositionsAsVectors).ToArray();
            var faceAxesA = a.Select(x => x.FaceNormal);
            var edgeAxesA = a.SelectMany(x => x.EdgesAxes);
            // Standardise vector representation with Positivise and eliminate duplicates
            // to reduce volume of normals to test
            var axesA = faceAxesA.Union(edgeAxesA).Select(x => Triangle3DHelper.Positivise(x)).Distinct().ToArray();

            var verticesB = b.SelectMany(x => x.PositionsAsVectors).ToArray();
            var faceAxesB = b.Select(x => x.FaceNormal);
            var edgeAxesB = b.SelectMany(x => x.EdgesAxes);
            // Standardise vector representation with Positivise and eliminate duplicates
            // to reduce volume of normals to test
            var axesB = faceAxesB.Union(edgeAxesB).Select(x => Triangle3DHelper.Positivise(x)).Distinct().ToArray();

            // Detect collision on each axes represented by A normals
            foreach (var axis in axesA)
            {
                if (AreSeparated(verticesA, verticesB, axis))
                {
                    return false;
                }
            }

            // Detect collision on each axes represented by B normals
            foreach (var axis in axesB)
            {
                if (AreSeparated(verticesA, verticesB, axis))
                {
                    return false;
                }
            }

            // Detect collision between each projected axes represented by each cross products  axisA * axisB
            foreach (var axisA in axesA)
            {
                foreach (var axisB in axesB)
                {
                    if (AreSeparated(verticesA, verticesB, Vector3D.CrossProduct(axisA, axisB)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool AreSeparated(Vector3D[] verticesA, Vector3D[] verticesB, Vector3D axis)
        {
            // Handles the cross product = {0,0,0} case
            if (axis == VectorZero)
            {
                return false;
            }

            // Define two intervals, a and b. Calculate their min and max values
            var aProjections = verticesA.Select(vertice => Vector3D.DotProduct(vertice, axis)).ToList();
            var aMin = aProjections.Min();
            var aMax = aProjections.Max();

            var bProjections = verticesB.Select(vertice => Vector3D.DotProduct(vertice, axis)).ToList();
            var bMin = bProjections.Min();
            var bMax = bProjections.Max();

            // One-dimensional intersection test between a and b
            var overlap = FindOverlap(aMin, aMax, bMin, bMax);
            var hasIntersect = aMax >= bMin && bMax >= aMin; // > to manage touching as intersection

            return !hasIntersect;
        }

        private double FindOverlap(double aMin, double aMax, double bMin, double bMax)
        {
            if (aMin < bMin)
            {
                if (aMax < bMin)
                {
                    return 0;
                }

                return aMax - bMin;
            }

            if (bMax < aMin)
            {
                return 0;
            }

            return bMax - aMin;
        }
    }
}
