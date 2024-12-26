using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public interface IMapRenderer
    {
        MapDefinition Definition { get; }

        List<LayeredModel3D> Render();
    }
}
