using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Definitions
{
    public class MapDefinition
    {
        public string Name { get; set; }
        public Point3D PlayerPosition { get; set; }
        public Vector3D PlayerLookDirection { get; set; }

        public List<LightDefinition> Lights { get; set; }
        public List<SkyboxDefinition> Skyboxes { get; set; }
        public List<VolumeDefinition> Volumes { get; set; }
    }
}
