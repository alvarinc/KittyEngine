using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Physics.Collisions.BVH;
using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    public class LoadMapBehavior : ServerBehavior
    {
        private ILogger _logger;
        private IMapBuilderFactory _mapBuilderFactory;
        private ILayeredModel3DFactory _layeredModel3DFactory;

        public LoadMapBehavior(ILogger logger, IMapBuilderFactory mapBuilderFactory, ILayeredModel3DFactory layeredModel3DFactory)
        {
            _logger = logger;
            _mapBuilderFactory = mapBuilderFactory;
            _layeredModel3DFactory = layeredModel3DFactory;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "loadmap")
            {
                return GameCommandResult.None;
            }

            var _mapName = input.Args["name"];

            if (context.Player != null)
            {
                _logger.Log(LogLevel.Info, $"[Server] Loadmap can only be executed at server level.");
                return GameCommandResult.None;
            }

            var factory = _mapBuilderFactory.Get(_mapName);
            var maps = _mapBuilderFactory.GetMaps();
            context.GameState.Map = factory.CreateMap();

            var leafNodefactory = new DefaultBVHLeafNodeFactory(_layeredModel3DFactory, context.GameState.Map.Volumes);
            var bvhTreeBuilder = new BVHTreeBuilder<LayeredModel3D>();
            context.GameState.MapBvhTree = bvhTreeBuilder.Build(leafNodefactory);

            context.GameState.Status = State.GameStatus.Created;
            _logger.Log(LogLevel.Info, $"[Server] Loaded map <{context.GameState.Map.Name}>. {maps.Length} maps");

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
