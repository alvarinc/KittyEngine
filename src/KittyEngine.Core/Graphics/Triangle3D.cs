using System;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public class Triangle3D
    {
        public Triangle3D(MeshGeometry3D geometry)
        {
            Initialize(
                geometry.Positions[geometry.TriangleIndices[0]],
                geometry.Positions[geometry.TriangleIndices[1]],
                geometry.Positions[geometry.TriangleIndices[2]]);
        }

        public Triangle3D(Point3D a, Point3D b, Point3D c)
        {
            Initialize(a, b, c);
        }

        public Point3D[] Positions { get; set; }

        public Vector3D[] PositionsAsVectors { get; set; }

        public Vector3D FaceNormal { get; set; }

        public Vector3D[] EdgesAxes { get; set; }

        public void ComputeNormalAndEdges()
        {
            var a = Positions[0];
            var b = Positions[1];
            var c = Positions[2];

            PositionsAsVectors = new Vector3D[3];
            PositionsAsVectors[0] = new Vector3D(a.X, a.Y, a.Z);
            PositionsAsVectors[1] = new Vector3D(b.X, b.Y, b.Z);
            PositionsAsVectors[2] = new Vector3D(c.X, c.Y, c.Z);

            var normal = Vector3D.CrossProduct(b - a, a - c);
            normal.Normalize();
            FaceNormal = normal;

            EdgesAxes = new Vector3D[3];
            EdgesAxes[0] = b - a;
            EdgesAxes[1] = c - b;
            EdgesAxes[2] = a - c;
        }

        private void Initialize(Point3D a, Point3D b, Point3D c)
        {
            Positions = new[] { a, b, c };
            ComputeNormalAndEdges();
        }

        public override string ToString()
        {
            return $"| {ToString("A", Positions[0])} | {ToString("B", Positions[1])} | {ToString("C", Positions[2])} |";
        }

        private string ToString(string pointName, Point3D point)
        {
            return $"{pointName}.X:{Math.Round(point.X, 4),8}; {pointName}.Y:{Math.Round(point.Y, 4),8}; {pointName}.Z:{Math.Round(point.Z, 4),8};";
        }

        public override bool Equals(object obj)
        {
            return obj is Triangle3D && GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return $"{Positions[0]} {Positions[1]} {Positions[2]}".GetHashCode();
        }

        internal Point3D GetHighestPoint()
        {
            return Positions.Aggregate((highest, current) => current.Y > highest.Y ? current : highest);
        }
    }
}
