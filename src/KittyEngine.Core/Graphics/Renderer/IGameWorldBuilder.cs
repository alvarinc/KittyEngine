using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.Graphics.Renderer
{
    public interface IGameWorldBuilder
    {
        List<LayeredModel3D> Create(MapDefinition mapDefinition);
    }
}
