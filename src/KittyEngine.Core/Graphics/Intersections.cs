using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public static class Intersections
    {
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
            var rectTriangles = Triangle3DHelper.CreateTriangles(rect);
            var intersectionPoints = new List<Point3D>();

            foreach (var triangle in triangles)
            {
                foreach (var rectTriangle in rectTriangles)
                {
                    var plane = new Plane3D(rectTriangle);
                    if (IntersectsPlane(plane, triangle, out var points))
                    {
                        if (points.Count == 1 && IsPointInsideRect3D(rect, points[0]))
                        {
                            intersectionPoints.Add(points[0]);
                        }
                        else if (points.Count == 2)
                        {
                            Point3D intersection1, intersection2;
                            if (HasIntersection(points[0], points[1], rectTriangle.Positions[0], rectTriangle.Positions[1], out intersection1, out intersection2))
                            {
                                intersectionPoints.AddRange(new[] { intersection1, intersection2 });
                            }

                            if (HasIntersection(points[0], points[1], rectTriangle.Positions[1], rectTriangle.Positions[2], out intersection1, out intersection2))
                            {
                                intersectionPoints.AddRange(new[] { intersection1, intersection2 });
                            }

                            if (HasIntersection(points[0], points[1], rectTriangle.Positions[2], rectTriangle.Positions[0], out intersection1, out intersection2))
                            {
                                intersectionPoints.AddRange(new[] { intersection1, intersection2 });
                            }
                        }
                    }
                }
            }

            return intersectionPoints.Distinct().ToList(); // Remove duplicates.
        }

        public static bool IsPointInsideRect3D(Rect3D rect, Point3D point)
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
                var verticesInsideRect = triangle.Positions.Where(vertex => IsPointInsideRect3D(rect, vertex)).ToList();
                insideVertices.AddRange(verticesInsideRect);
            }

            return insideVertices.Distinct().ToList(); // Remove duplicates
        }

        public static bool HasIntersection(Point3D p1, Point3D p2, Point3D q1, Point3D q2, out Point3D intersectionStart, out Point3D intersectionEnd)
        {
            intersectionStart = new Point3D(0, 0, 0);
            intersectionEnd = new Point3D(0, 0, 0);

            // Directions of the lines
            Vector3D d1 = p2 - p1;
            Vector3D d2 = q2 - q1;

            // Check if the lines are collinear
            if (Vector3D.CrossProduct(d1, d2).LengthSquared < 1e-6)
            {
                // Check if they are collinear by testing one point
                if (Vector3D.CrossProduct(p2 - p1, q1 - p1).LengthSquared < 1e-6)
                {
                    // Collinear - check for overlap
                    Point3D minP = p1, maxP = p2;
                    if (ComparePoints(maxP, minP) < 0)
                    {
                        // Swap minP and maxP
                        Point3D temp = minP;
                        minP = maxP;
                        maxP = temp;
                    }

                    Point3D minQ = q1, maxQ = q2;
                    if (ComparePoints(maxQ, minQ) < 0)
                    {
                        // Swap minQ and maxQ
                        Point3D temp = minQ;
                        minQ = maxQ;
                        maxQ = temp;
                    }

                    // Find the overlap
                    Point3D overlapStart = Max(minP, minQ);
                    Point3D overlapEnd = Min(maxP, maxQ);

                    // Check if there is a valid overlap
                    if (ComparePoints(overlapStart, overlapEnd) <= 0)
                    {
                        intersectionStart = overlapStart;
                        intersectionEnd = overlapEnd;
                        return true; // Overlapping segment found
                    }
                }
                return false; // Collinear but disjoint
            }

            // Non-collinear case (original code)
            Vector3D r = p1 - q1;
            double a = Vector3D.DotProduct(d1, d1);
            double b = Vector3D.DotProduct(d1, d2);
            double c = Vector3D.DotProduct(d2, d2);
            double d = Vector3D.DotProduct(d1, r);
            double e = Vector3D.DotProduct(d2, r);

            double denominator = a * c - b * b;

            if (Math.Abs(denominator) < 1e-6)
            {
                return false; // Parallel but not collinear
            }

            double t = (b * e - c * d) / denominator;
            double u = (a * e - b * d) / denominator;

            if (t < 0 || t > 1 || u < 0 || u > 1)
            {
                return false; // Segments do not intersect
            }

            Point3D closestPointOnLine1 = p1 + t * d1;
            Point3D closestPointOnLine2 = q1 + u * d2;

            if ((closestPointOnLine1 - closestPointOnLine2).Length > 1e-6)
            {
                return false; // Closest points are too far apart
            }

            intersectionStart = closestPointOnLine1;
            intersectionEnd = closestPointOnLine1;
            return true; // Single intersection point
        }

        private static int ComparePoints(Point3D a, Point3D b)
        {
            // Compare lexicographically by X, then Y, then Z
            if (a.X < b.X) return -1;
            if (a.X > b.X) return 1;
            if (a.Y < b.Y) return -1;
            if (a.Y > b.Y) return 1;
            if (a.Z < b.Z) return -1;
            if (a.Z > b.Z) return 1;
            return 0; // Points are equal
        }

        private static Point3D Max(Point3D a, Point3D b) =>
            new Point3D(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));

        private static Point3D Min(Point3D a, Point3D b) =>
            new Point3D(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
    }
}
