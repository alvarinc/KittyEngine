using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Server.Commands
{
    internal class LoadMapCommand : IGameCommand
    {
        private ILogger _logger;
        private IMapBuilderFactory _mapBuilderFactory;
        private string _mapName;

        public LoadMapCommand(ILogger logger, IMapBuilderFactory mapBuilderFactory)
        {
            _logger = logger;
            _mapBuilderFactory = mapBuilderFactory;
        }

        public bool ValidateParameters(GameCommandInput cmd)
        {
            _mapName = cmd.Args["name"];
            return true;
        }

        public GameCommandResult Execute(GameCommandContext context)
        {
            if (context.Player != null)
            {
                _logger.Log(LogLevel.Info, $"[Server] Loadmap can only be executed at server level.");
            }

            var factory = _mapBuilderFactory.Get(_mapName);
            var maps = _mapBuilderFactory.GetMaps();
            context.GameState.Map = factory.CreateMap();
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
