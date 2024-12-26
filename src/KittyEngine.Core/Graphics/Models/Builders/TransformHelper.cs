using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Builders
{
    public class TransformHelper
    {
        public static double NormalizeAngle(double angle)
        {
            double normalizedDeg = angle % 360;

            if (normalizedDeg <= -180)
                normalizedDeg += 360;
            else if (normalizedDeg > 180)
                normalizedDeg -= 360;

            return normalizedDeg;
        }

        public static Transform3D TransformByXAxisRotation(double d, LayeredModel3D layeredModel)
        {
            var points = GetPoints(layeredModel.Children);
            var origin = Barycenter(points);
            return TransformByXAxisRotation(d, origin);
        }

        public static Transform3D TransformByXAxisRotation(double d, Point3D? origin = null)
        {
            return TransformByAxisRotation(new Vector3D(1, 0, 0), d, origin);
        }

        public static Transform3D TransformByYAxisRotation(double d, LayeredModel3D layeredModel)
        {
            var points = GetPoints(layeredModel.Children);
            var origin = Barycenter(points);
            return TransformByYAxisRotation(d, origin);
        }

        public static Transform3D TransformByYAxisRotation(double d, Point3D? origin = null)
        {
            return TransformByAxisRotation(new Vector3D(0, 1, 0), d, origin);
        }

        public static Transform3D TransformByZAxisRotation(double d, LayeredModel3D layeredModel)
        {
            var points = GetPoints(layeredModel.Children);
            var origin = Barycenter(points);
            return TransformByZAxisRotation(d, origin);
        }

        public static Transform3D TransformByZAxisRotation(double d, Point3D? origin = null)
        {
            return TransformByAxisRotation(new Vector3D(0, 0, 1), d, origin);
        }

        public static Transform3D Scale(double d, Point3D? origin = null)
        {
            if (origin == null)
            {
                origin = new Point3D(0, 0, 0);
            }

            return new ScaleTransform3D(d, d, d, origin.Value.X, origin.Value.Y, origin.Value.Z);
        }

        public static Transform3D TransformByAxisRotation(Vector3D axis, double d, Point3D? origin = null)
        {
            if (origin == null)
            {
                origin = new Point3D(0, 0, 0);
            }

            var axisRotation = new AxisAngleRotation3D(axis, d);
            return new RotateTransform3D(axisRotation, origin.Value);
        }

        public static Transform3D TransformByTranslation(double x, double y, double z)
        {
            var matrix = Matrix3D.Identity;
            var vector = new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            vector += new Vector3D(x, y, z);

            matrix.OffsetX = vector.X;
            matrix.OffsetY = vector.Y;
            matrix.OffsetZ = vector.Z;

            return new MatrixTransform3D(matrix);
        }

        public static Point3D Barycenter(List<Point3D> points)
        {
            var x = points.Sum(x => x.X) / points.Count();
            var y = points.Sum(x => x.Y) / points.Count();
            var z = points.Sum(x => x.Z) / points.Count();

            return new Point3D(x, y, z);
        }

        public static Rect3D Bounds(List<Point3D> points)
        {
            var Xmin = points.Min(x => x.X);
            var Xmax = points.Max(x => x.X);
            var Ymin = points.Min(x => x.Y);
            var Ymax = points.Max(x => x.Y);
            var Zmin = points.Min(x => x.Z);
            var Zmax = points.Max(x => x.Z);

            return new Rect3D(Xmin, Ymin, Zmin, Xmax-Xmin, Ymax-Ymin, Zmax-Zmin);
        }

        public static List<Point3D> GetPoints(Model3DCollection modelCollection)
        {
            var points = new List<Point3D>();
            foreach (var model in modelCollection)
            {
                points.AddRange(GetPoints(model));
            }

            return points;
        }

        public static List<Point3D> GetPoints(Model3D model)
        {
            var points = new List<Point3D>();
            if (model is Model3DGroup)
            {
                foreach (var childModel in ((Model3DGroup)model).Children)
                {
                    points.AddRange(GetPoints(childModel));
                }
            }
            else
            {
                var geometryModel = (GeometryModel3D)model;
                var mesh = geometryModel.Geometry as MeshGeometry3D;

                foreach (var indice in mesh.TriangleIndices)
                {
                    points.Add(mesh.Positions[indice]);
                }
            }

            return points;
        }

        public static List<Point3D[]> GetPointsByMeshGeometry3D(Model3DCollection modelCollection)
        {
            var points = new List<Point3D[]>();
            foreach (var model in modelCollection)
            {
                points.AddRange(GetPointsByMeshGeometry3D(model));
            }

            return points;
        }

        public static List<Point3D[]> GetPointsByMeshGeometry3D(Model3D model)
        {
            var pointsByMeshes = new List<Point3D[]>();
            if (model is Model3DGroup)
            {
                foreach (var childModel in ((Model3DGroup)model).Children)
                {
                    pointsByMeshes.AddRange(GetPointsByMeshGeometry3D(childModel));
                }
            }
            else
            {
                var geometryModel = (GeometryModel3D)model;
                var mesh = geometryModel.Geometry as MeshGeometry3D;
                var points = new List<Point3D>();
                foreach (var indice in mesh.TriangleIndices)
                {
                    points.Add(mesh.Positions[indice]);
                }

                pointsByMeshes.Add(points.ToArray());
            }

            return pointsByMeshes;
        }
    }
}
