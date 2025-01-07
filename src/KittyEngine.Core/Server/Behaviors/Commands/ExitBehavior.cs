using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    internal class ExitBehavior : ServerBehavior
    {
        private ILogger _logger;

        public ExitBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "exit")
            {
                return GameCommandResult.None;
            }

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
