using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.State
{
    public class GraphicState
    {
        public List<LayeredModel3D> Map { get; set; } = new();

        public Dictionary<int, LayeredModel3D> Players { get; set; } = new();
    }
}
