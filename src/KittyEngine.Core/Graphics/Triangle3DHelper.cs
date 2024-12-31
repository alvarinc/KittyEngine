using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public static class Triangle3DHelper
    {
        public static List<Vector3D> GetDuplicateEdges(List<Triangle3D> triangles)
        {
            var duplicates = new List<Vector3D>();

            var edges = triangles.SelectMany(x => x.EdgesAxes).Select(x => Positivise(x));

            foreach(var aEdge in edges)
            {
                foreach(var bEdge in edges)
                {
                    if (aEdge == bEdge && !duplicates.Contains(aEdge))
                    {
                        duplicates.Add(aEdge);
                    }
                }
            }

            return duplicates;
        }

        public static Vector3D Positivise(Vector3D vector)
        {
            var negativeDimentions =
                (vector.X < 0 ? 1 : 0) +
                (vector.Y < 0 ? 1 : 0) +
                (vector.Z < 0 ? 1 : 0);

            var transformed = vector;
            if (negativeDimentions > 1)
            {
                transformed.Negate();
            }

            // Convert possible -0 values to 0
            if (transformed.X == 0)
            {
                transformed.X = 0;
            }

            if (transformed.Y == 0)
            {
                transformed.Y = 0;
            }

            if (transformed.Z == 0)
            {
                transformed.Z = 0;
            }
            return transformed;
        }

        public static List<Triangle3D> CreateLine(Point3D startPoint, Point3D endPoint)
        {
            double halfWdth = .1;
            Point3D p0 = new Point3D(startPoint.X, startPoint.Y - halfWdth, startPoint.Z - halfWdth);
            Point3D p1 = new Point3D(endPoint.X, endPoint.Y - halfWdth, endPoint.Z - halfWdth);
            Point3D p2 = new Point3D(endPoint.X, endPoint.Y - halfWdth, endPoint.Z + halfWdth);
            Point3D p3 = new Point3D(startPoint.X, startPoint.Y - halfWdth, startPoint.Z + halfWdth);
            Point3D p4 = new Point3D(startPoint.X, startPoint.Y + halfWdth, startPoint.Z - halfWdth);
            Point3D p5 = new Point3D(endPoint.X, endPoint.Y + halfWdth, endPoint.Z - halfWdth);
            Point3D p6 = new Point3D(endPoint.X, endPoint.Y + halfWdth, endPoint.Z + halfWdth);
            Point3D p7 = new Point3D(startPoint.X, startPoint.Y + halfWdth, startPoint.Z + halfWdth);

            var meshes = new List<Triangle3D>();

            //front
            meshes.Add(new Triangle3D(p3, p2, p6));
            meshes.Add(new Triangle3D(p3, p6, p7));

            //right
            meshes.Add(new Triangle3D(p2, p1, p5));
            meshes.Add(new Triangle3D(p2, p5, p6));

            //back
            meshes.Add(new Triangle3D(p1, p0, p4));
            meshes.Add(new Triangle3D(p1, p4, p5));

            //left
            meshes.Add(new Triangle3D(p0, p3, p7));
            meshes.Add(new Triangle3D(p0, p7, p4));

            //top
            meshes.Add(new Triangle3D(p7, p6, p5));
            meshes.Add(new Triangle3D(p7, p5, p4));

            //bottom
            meshes.Add(new Triangle3D(p2, p3, p0));
            meshes.Add(new Triangle3D(p2, p0, p1));

            return meshes;
        }

        public static List<Triangle3D> CreateTriangles(Rect3D rect)
        {
            Point3D p0 = new Point3D(rect.X, rect.Y, rect.Z);
            Point3D p1 = new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z);
            Point3D p2 = new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z + rect.SizeZ);
            Point3D p3 = new Point3D(rect.X, rect.Y, rect.Z + rect.SizeZ);
            Point3D p4 = new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z);
            Point3D p5 = new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z);
            Point3D p6 = new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);
            Point3D p7 = new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);

            var meshes = new List<Triangle3D>();

            //front
            meshes.Add(new Triangle3D(p3, p2, p6));
            meshes.Add(new Triangle3D(p3, p6, p7));

            //right
            meshes.Add(new Triangle3D(p2, p1, p5));
            meshes.Add(new Triangle3D(p2, p5, p6));

            //back
            meshes.Add(new Triangle3D(p1, p0, p4));
            meshes.Add(new Triangle3D(p1, p4, p5));

            //left
            meshes.Add(new Triangle3D(p0, p3, p7));
            meshes.Add(new Triangle3D(p0, p7, p4));

            //top
            meshes.Add(new Triangle3D(p7, p6, p5));
            meshes.Add(new Triangle3D(p7, p5, p4));

            //bottom
            meshes.Add(new Triangle3D(p2, p3, p0));
            meshes.Add(new Triangle3D(p2, p0, p1));

            return meshes;
        }

        public static List<Triangle3D> CreateTriangles(Model3DCollection modelCollection)
        {
            return GetMeshes(modelCollection)
                .Select(mesh => new Triangle3D(mesh))
                .ToList();
        }

        private static List<MeshGeometry3D> GetMeshes(Model3DCollection modelCollection)
        {
            var meshes = new List<MeshGeometry3D>();
            foreach (var model in modelCollection)
            {
                meshes.AddRange(GetMeshes(model));
            }

            return meshes;
        }

        private static List<MeshGeometry3D> GetMeshes(Model3D model)
        {
            var meshes = new List<MeshGeometry3D>();
            if (model is Model3DGroup)
            {
                foreach (var childModel in ((Model3DGroup)model).Children)
                {
                    meshes.AddRange(GetMeshes(childModel));
                }
            }
            else
            {
                var geometryModel = (GeometryModel3D)model;
                var mesh = geometryModel.Geometry as MeshGeometry3D;
                meshes.Add(mesh);
            }

            return meshes;
        }
    }
}
