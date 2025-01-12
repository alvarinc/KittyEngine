using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Physics;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal interface IGameWorldRenderer
    {
        void RegisterOutput(IGameHost host);
        void LoadGameWorld(MapDefinition mapDefinition);
        void UpdateCamera();
        void UpdatePlayers();
    }
}