using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Assets.Maps.Predefined
{
    public class NewMapBuilder : IMapBuilder
    {
        public string MapName => PredefinedMapNames.New;

        public Point3D PlayerPosition => new Point3D(10, 1, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public MapDefinition CreateMap()
        {
            var level = new MapDefinition();

            // Lights
            level.Lights = new List<LightDefinition>();
            level.Lights.Add(new LightDefinition
            {
                Color = Colors.Transparent,
                Metadata = new LightMetadata
                {
                    Direction = new Vector3D(-3, -4, -5)
                }
            });

            level.Lights.Add(new LightDefinition
            {
                Color = Colors.Transparent,
                Metadata = new LightMetadata
                {
                    Direction = new Vector3D(3, 4, 5)
                }
            });

            // Sky box
            level.Skyboxes = new List<SkyboxDefinition>();
            level.Skyboxes.Add(new SkyboxDefinition
            {
                Color = Colors.Black,
                Position = new Point3D(-5000, -5000, -5000),
                Metadata = new SkyboxMetadata { Normal = new Vector3D(-3, -4, -5), Size = 10000 }
            });

            // Origin
            level.Volumes = new List<VolumeDefinition>();
            level.Volumes.Add(CreateVolume(Colors.White, 0, 0, 0, 0.5));
            level.Volumes.Add(CreateVolume(Colors.LightGreen, 0, 0.5, 0, 0.5, 15, 0.5));
            level.Volumes.Add(CreateVolume(Colors.Red, 0.5, 0, 0, 15, 0.5, 0.5));
            level.Volumes.Add(CreateVolume(Colors.Blue, 0, 0, 0.5, 0.5, 0.5, 15));

            // Floor : central
            var midFloorSize = 100;

            // floor
            level.Volumes.Add(CreateVolume(Colors.LightGray, -midFloorSize, -1, -midFloorSize, midFloorSize * 2, 1, midFloorSize * 2));

            // top
            level.Volumes.Add(CreateVolume(Colors.LightGray, -midFloorSize, midFloorSize * 2, -midFloorSize, midFloorSize * 2, 1, midFloorSize * 2));

            // front
            level.Volumes.Add(CreateVolume(Colors.LightGray, -midFloorSize, 0, -midFloorSize, midFloorSize * 2, midFloorSize * 2, 1));

            // back
            level.Volumes.Add(CreateVolume(Colors.LightGray, -midFloorSize, 0, midFloorSize, midFloorSize * 2, midFloorSize * 2, 1));

            // left
            level.Volumes.Add(CreateVolume(Colors.LightGray, -midFloorSize, 0, -midFloorSize, 1, midFloorSize * 2, midFloorSize * 2));

            // right
            level.Volumes.Add(CreateVolume(Colors.LightGray, midFloorSize, 0, -midFloorSize, 1, midFloorSize * 2, midFloorSize * 2));

            return level;
        }

        private VolumeDefinition CreateVolume(Color color, double x, double y, double z, double size)
        {
            return CreateVolume(color, x, y, z, size, size, size);
        }

        private VolumeDefinition CreateVolume(Color color, double x, double y, double z, double xSize, double ySize, double zSize)
        {
            return new VolumeDefinition
            {
                Color = color,
                Position = new Point3D(x, y, z),
                Metadata = new VolumeMetadata
                {
                    UseBackMaterial = true,
                    XSize = xSize,
                    YSize = ySize,
                    ZSize = zSize,
                }
            };
        }
    }
}
