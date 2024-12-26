using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Definitions
{
    public class SkyboxMetadata
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Vector3D Normal { get; set; }
        public bool UseBackMaterial { get; set; }
    }
}
