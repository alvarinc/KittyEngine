using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics
{
    public class Rect3DHelper
    {
        public static Point3D GetCenter(Rect3D bounds)
        {
            return new Point3D(
                bounds.X + bounds.SizeX / 2,
                bounds.Y + bounds.SizeY / 2,
                bounds.Z + bounds.SizeZ / 2);
        }

        public static Rect3D CreateBoundingBox(IEnumerable<Rect3D> boundObjects)
        {
            var first = boundObjects.First();
            var minX = boundObjects.Min(x => x.X);
            var minY = boundObjects.Min(x => x.Y);
            var minZ = boundObjects.Min(x => x.Z);
            var maxX = boundObjects.Max(x => x.X + x.SizeX);
            var maxY = boundObjects.Max(x => x.Y + x.SizeY);
            var maxZ = boundObjects.Max(x => x.Z + x.SizeZ);

            return new Rect3D()
            {
                Location = new Point3D(minX, minY, minZ),
                SizeX = maxX - minX,
                SizeY = maxY - minY,
                SizeZ = maxZ - minZ,
            };
        }
    }
}
