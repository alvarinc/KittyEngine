using KittyEngine.Core.Graphics.Models.Builders;
using System.Windows.Media;

namespace KittyEngine.Core.Graphics.Models.Definitions
{
    public class ModelMetadata
    {
        public ModelType Type;
        public Color Color { get; set; }
        public bool UseBackMaterial { get; set; }
    }
}
