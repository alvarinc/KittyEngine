using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Physics;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal interface IMapRenderer
    {
        void BindGraphicsToViewport(IGameHost host);
        void LoadMap(MapDefinition mapDefinition);
        void UpdateCamera(IMovableBody body);
    }
}