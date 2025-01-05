using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public class Plane3D
    {
        public Point3D Point { get; set; }    // A point on the plane
        public Vector3D Normal { get; set; } // The normal vector of the plane

        // Constructor: Define a plane with a Point3D and a normal vector
        public Plane3D(Point3D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
            NormalizeNormal();
        }

        // Constructor: Define a plane with a Triangle3D
        public Plane3D(Triangle3D triangle)
        {
            Point = triangle.Positions[0];
            Normal = triangle.FaceNormal;
            NormalizeNormal();
        }

        // Ensure the normal vector is normalized
        private void NormalizeNormal()
        {
            if (Normal.Length != 1)
            {
                Normal.Normalize();
            }
        }

        // Calculate the signed distance from a point to the plane
        public double Distance(Point3D point)
        {
            var vector = point - Point; // Vector from the plane's point to the input point
            return Vector3D.DotProduct(Normal, vector); // Dot product gives the signed distance
        }
    }
}
