using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Graphics.Assets.Maps.Predefined;
using KittyEngine.Core.Server.Model;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal class LoadMapCommand : GameCommandBase
    {
        private ILogger _logger;
        private IMapBuilderFactory _mapBuilderFactory;
        private string _mapName;

        public LoadMapCommand(ILogger logger, IMapBuilderFactory mapBuilderFactory)
        {
            _logger = logger;
            _mapBuilderFactory = mapBuilderFactory;
        }

        public override bool ValidateParameters(GameCommandInput cmd)
        {
            _mapName = cmd.Args["name"];
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player)
        {
            if (player != null)
            {
                _logger.Log(LogLevel.Info, $"[Server] Loadmap can only be executed at server level.");
            }

            var factory = _mapBuilderFactory.Get(_mapName);
            var maps = _mapBuilderFactory.GetMaps();
            gameState.Map = factory.CreateMap();
            _logger.Log(LogLevel.Info, $"[Server] Loaded map <{gameState.Map.Name}>. {maps.Length} maps");

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
