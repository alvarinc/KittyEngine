using KittyEngine.Core;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Services.IoC;
using KittyEngine.SampleMaps.Maze;

namespace KittyEngine.SampleMaps
{
    public class RegisterSampleMapsBehavior : CompositionBehavior
    {
        private EngineRuntime _runtime;

        public RegisterSampleMapsBehavior(EngineRuntime runtime) 
        {
            _runtime = runtime;
        }

        public override void OnConfigureServices(IServiceContainer container)
        {
            var contentService = container.Get<IContentService>();
            contentService.RegisterContentFromSampleMaps();

            if (_runtime == EngineRuntime.Server)
            {
                var mapBuilderFactory = container.Get<IMapBuilderFactory>();
                mapBuilderFactory.RegisterMapsFromAssets();
                mapBuilderFactory.RegisterMap(new MazeMapBuilder());
            }
        }
    }
}
