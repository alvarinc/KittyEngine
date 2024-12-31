using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal class JoinCommand : IGameCommand
    {
        private ILogger _logger;

        private string _guid;
        private string _name;

        public JoinCommand(ILogger logger)
        {
            _logger = logger;
        }

        public bool ValidateParameters(GameCommandInput input)
        {
            _guid = input.Args["guid"];
            _name = input.Args["name"];
            return true;
        }

        public GameCommandResult Execute(GameCommandContext context)
        {
            var player = context.Player;
            player.Guid = _guid;
            player.Name = _name;

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
