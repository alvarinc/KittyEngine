using KittyEngine.Core.Graphics.Models.Definitions;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Assets.Maps.Predefined
{
    public class EmptyMapBuilder : IMapBuilder
    {
        public string MapName => PredefinedMapNames.Empty;

        public Point3D PlayerPosition => new Point3D(10, 11, 70);

        public Vector3D PlayerLookDirection => new Vector3D(0, 0, -1);

        public MapDefinition CreateMap()
        {
            var level = new MapDefinition();

            level.Skyboxes = new List<SkyboxDefinition>();
            level.Lights = new List<LightDefinition>();
            level.Volumes = new List<VolumeDefinition>();

            return level;
        }
    }
}
