using KittyEngine.Core;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Services.IoC;
using KittyEngine.SampleMaps.Maze;
using System.Reflection;

namespace KittyEngine.SampleMaps
{
    public class RegisterSampleAssetsBehavior : CompositionBehavior
    {
        public override void OnConfigureServices(IServiceContainer container)
        {
            container.Get<IContentService>().RegisterSource(new EmbeddedContentSource(Assembly.GetExecutingAssembly()));
        }
    }

    public class RegisterSampleMapsBehavior : CompositionBehavior
    {
        public override void OnConfigureServices(IServiceContainer container)
        {
            container.Get<IMapBuilderFactory>().RegisterMap(new MazeMapBuilder());
        }
    }
}
