using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public static class Intersections
    {
        public static List<Plane3D> GetPlanesFromRect3D(Rect3D rect)
        {
            var origin = new Point3D(rect.X, rect.Y, rect.Z);
            var size = new Vector3D(rect.SizeX, rect.SizeY, rect.SizeZ);

            // Define all corners of the Rect3D
            var vertices = new[]
            {
                origin,
                new Point3D(origin.X + size.X, origin.Y, origin.Z),
                new Point3D(origin.X, origin.Y + size.Y, origin.Z),
                new Point3D(origin.X, origin.Y, origin.Z + size.Z),
                new Point3D(origin.X + size.X, origin.Y + size.Y, origin.Z),
                new Point3D(origin.X, origin.Y + size.Y, origin.Z + size.Z),
                new Point3D(origin.X + size.X, origin.Y, origin.Z + size.Z),
                new Point3D(origin.X + size.X, origin.Y + size.Y, origin.Z + size.Z),
            };

            return new List<Plane3D>
            {
                // Front
                new Plane3D(vertices[0], new Vector3D(0, 0, -1)),
                // Back
                new Plane3D(vertices[7], new Vector3D(0, 0, 1)),
                // Left
                new Plane3D(vertices[0], new Vector3D(-1, 0, 0)),
                // Right
                new Plane3D(vertices[7], new Vector3D(1, 0, 0)),
                // Bottom
                new Plane3D(vertices[0], new Vector3D(0, -1, 0)),
                // Top
                new Plane3D(vertices[7], new Vector3D(0, 1, 0)),
            };
        }

        public static bool IntersectsPlane(Plane3D plane, Triangle3D triangle, out List<Point3D> intersectionPoints)
        {
            intersectionPoints = new List<Point3D>();

            var distances = triangle.Positions.Select(p => plane.Distance(p)).ToArray();

            bool hasPositive = distances.Any(d => d > 0);
            bool hasNegative = distances.Any(d => d < 0);

            if (!(hasPositive && hasNegative))
                return false; // No intersection.

            for (int i = 0; i < 3; i++)
            {
                var start = triangle.Positions[i];
                var end = triangle.Positions[(i + 1) % 3];
                var startDist = distances[i];
                var endDist = distances[(i + 1) % 3];

                if (startDist * endDist < 0) // Edge crosses the plane.
                {
                    double t = startDist / (startDist - endDist);
                    var intersectionPoint = new Point3D(
                        start.X + t * (end.X - start.X),
                        start.Y + t * (end.Y - start.Y),
                        start.Z + t * (end.Z - start.Z)
                    );
                    intersectionPoints.Add(intersectionPoint);
                }
            }

            return intersectionPoints.Count > 0;
        }

        public static List<Point3D> GetIntersectionPoints(Rect3D rect, List<Triangle3D> triangles)
        {
            var planes = GetPlanesFromRect3D(rect);
            var intersectionPoints = new List<Point3D>();

            foreach (var triangle in triangles)
            {
                foreach (var plane in planes)
                {
                    if (IntersectsPlane(plane, triangle, out var points))
                    {
                        intersectionPoints.AddRange(points.Where(x => IsPointInsideRect3D(x, rect)));
                    }
                }
            }

            return intersectionPoints.Distinct().ToList(); // Remove duplicates.
        }

        public static bool IsPointInsideRect3D(Point3D point, Rect3D rect)
        {
            return point.X >= rect.X && point.X <= rect.X + rect.SizeX &&
                   point.Y >= rect.Y && point.Y <= rect.Y + rect.SizeY &&
                   point.Z >= rect.Z && point.Z <= rect.Z + rect.SizeZ;
        }

        public static List<Point3D> GetVerticesInsideRect3D(Rect3D rect, List<Triangle3D> triangles)
        {
            var insideVertices = new List<Point3D>();

            foreach (var triangle in triangles)
            {
                var verticesInsideRect = triangle.Positions.Where(vertex => IsPointInsideRect3D(vertex, rect)).ToList();
                insideVertices.AddRange(verticesInsideRect);
            }

            return insideVertices.Distinct().ToList(); // Remove duplicates
        }
    }
}
