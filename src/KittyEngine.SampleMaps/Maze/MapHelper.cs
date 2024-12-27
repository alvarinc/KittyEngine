using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.SampleMaps.Maze
{
    public static class MapHelper
    {
        public static VolumeDefinition CreateVolume(Color color, double x, double y, double z, double size)
        {
            return CreateVolume(color, x, y, z, size, size, size);
        }

        public static VolumeDefinition CreateVolume(Color color, double x, double y, double z, double xSize, double ySize, double zSize)
        {
            return new VolumeDefinition
            {
                Color = color,
                Position = new Point3D(x, y, z),
                Metadata = new VolumeMetadata
                {
                    UseBackMaterial = false,
                    XSize = xSize,
                    YSize = ySize,
                    ZSize = zSize,
                }
            };
        }
    }
}
