using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public static class Geometry3D
    {
        private class NearestTriangleDescription
        {
            public Triangle3D Triangle { get; set; }
            public double Distance { get; set; }
        }

        public static IEnumerable<Triangle3D> SortTrianglesByNearest(IEnumerable<Triangle3D> triangles, Point3D externalPoint)
        {
            var list = new List<NearestTriangleDescription>();
            foreach (var triangle in triangles)
            {
                var point = NearestPointOnTriangle(triangle, externalPoint);
                var distance = (externalPoint - point).Length;

                list.Add(new NearestTriangleDescription { Triangle = triangle, Distance = distance });
            }

            return list.OrderBy(x => x.Distance).Select(x => x.Triangle);
        }

        public static Point3D NearestPointOnTriangle(Triangle3D triangle, Point3D externalPoint)
        {
            // Project on plane
            var pointOnPlane = ProjectOnPlane(triangle.Positions[0], triangle.Positions[1], triangle.Positions[2], externalPoint);

            // Check if point is into the triangle
            var projection1 = ProjectOnEdge(triangle.Positions[0], triangle.Positions[1], pointOnPlane);
            var projection2 = ProjectOnEdge(triangle.Positions[1], triangle.Positions[2], pointOnPlane);
            var projection3 = ProjectOnEdge(triangle.Positions[2], triangle.Positions[0], pointOnPlane);

            if (projection1.HasValue && projection2.HasValue && projection3.HasValue)
            {
                return pointOnPlane;
            }

            // Project on edges
            var nearestPoint1 = NearestPointOnEdge(triangle.Positions[0], triangle.Positions[1], externalPoint);
            var nearestPoint2 = NearestPointOnEdge(triangle.Positions[1], triangle.Positions[2], externalPoint);
            var nearestPoint3 = NearestPointOnEdge(triangle.Positions[2], triangle.Positions[0], externalPoint);

            // Get closest edge projection
            var nearestDistance1 = (externalPoint - nearestPoint1).Length;
            var nearestDistance2 = (externalPoint - nearestPoint2).Length;
            var nearestDistance3 = (externalPoint - nearestPoint3).Length;

            var nearestPoint = nearestPoint1;
            var nearestDistance = nearestDistance1;
            if (nearestDistance2 < nearestDistance)
            {
                nearestPoint = nearestPoint2;
                nearestDistance = nearestDistance1;
            }

            if (nearestDistance3 < nearestDistance)
            {
                nearestPoint = nearestPoint3;
            }

            return nearestPoint;
        }

        public static Vector3D ProjectOnPlane(Triangle3D plane, Vector3D projected)
        {
            // Calculate the plane's normal vector
            Vector3D planeNormal = plane.FaceNormal;
            planeNormal.Normalize();

            // Calculate the projection of the vector onto the plane
            double dotProduct = Vector3D.DotProduct(projected, planeNormal);
            Vector3D perpendicularComponent = dotProduct * planeNormal;

            // Subtract the perpendicular component from the original vector
            Vector3D projectedVector = projected - perpendicularComponent;

            return projectedVector;
        }

        public static Point3D ProjectOnPlane(Triangle3D plane, Point3D projected)
        {
            return ProjectOnPlane(plane.Positions[0], plane.Positions[1], plane.Positions[2], projected);
        }

        public static Point3D ProjectOnPlane(Point3D planePointA, Point3D planePointB, Point3D planePointC, Point3D projectedPoint)
        {
            // Calculate normal
            var planeVector1 = planePointB - planePointA;
            var planeVector2 = planePointC - planePointA;
            var planeNormal = Vector3D.CrossProduct(planeVector1, planeVector2);
            planeNormal.Normalize();

            // Calculate the distance from the point to the plane:
            var distance = Vector3D.DotProduct(planeNormal, planePointA - projectedPoint);

            // Translate the point to form a projection
            return projectedPoint + planeNormal * distance;
        }

        public static Point3D ProjectOnLine(Point3D edgeStartPoint, Point3D edgeEndPoint, Point3D projected)
        {
            var linedirection = edgeEndPoint - edgeStartPoint;
            linedirection.Normalize();//this needs to be a unit vector
            var d = Vector3D.DotProduct(projected - edgeStartPoint, linedirection);

            return edgeStartPoint + linedirection * d;
        }

        public static Point3D? ProjectOnEdge(Point3D edgeStartPoint, Point3D edgeEndPoint, Point3D projected)
        {
            var linedirection = edgeEndPoint - edgeStartPoint;
            var edgeSize = linedirection.Length;
            linedirection.Normalize();//this needs to be a unit vector
            var d = Vector3D.DotProduct(projected - edgeStartPoint, linedirection);

            if (d < 0 || d > edgeSize)
            {
                return null;
            }

            return edgeStartPoint + linedirection * d;
        }

        public static Point3D NearestPointOnEdge(Point3D edgeStartPoint, Point3D edgeEndPoint, Point3D projected)
        {
            var linedirection = edgeEndPoint - edgeStartPoint;
            var edgeSize = linedirection.Length;
            linedirection.Normalize();//this needs to be a unit vector
            var d = Vector3D.DotProduct(projected - edgeStartPoint, linedirection);

            if (d < 0)
            {
                return edgeStartPoint;
            }
            else if (d > edgeSize)
            {
                return edgeEndPoint;
            }

            return edgeStartPoint + linedirection * d;
        }
    }
}
