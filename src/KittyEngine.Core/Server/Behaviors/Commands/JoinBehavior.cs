using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    public class JoinBehavior : ServerBehavior
    {
        private ILogger _logger;

        public JoinBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "join")
            {
                return GameCommandResult.None;
            }

            var guid = input.Args["guid"];
            var name = input.Args["name"];

            var player = context.Player;
            player.Guid = guid;
            player.Name = name;

            var playerState = new PlayerState(player.PeerId);
            playerState.Name = player.Name;
            playerState.Guid = player.Guid;
            playerState.Position = context.GameState.Map.PlayerPosition;
            playerState.LookDirection = context.GameState.Map.PlayerLookDirection;

            context.GameState.Players.Add(player.PeerId, playerState);

            _logger.Log(LogLevel.Info, $"[Server] Player {player.PeerId} : {player.Name} joined the game");

            var command = new GameCommandInput("joined")
              .AddArgument("guid", player.Guid)
              .AddArgument("name", player.Name);

            context.SendMessage(player.PeerId, command);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
