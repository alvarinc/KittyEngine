using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Server.Commands
{
    internal class ExitCommand : IGameCommand
    {
        private ILogger _logger;

        public ExitCommand(ILogger logger)
        {
            _logger = logger;
        }

        public bool ValidateParameters(GameCommandInput cmd)
        {
            return true;
        }

        public GameCommandResult Execute(GameCommandContext context)
        {
            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {context.Player.Name} requested to stop. Remove from game.");
            context.GameState.Players.Remove(context.Player.PeerId);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = false,
            };
        }
    }
}
