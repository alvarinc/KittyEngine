using KittyEngine.Core.Server;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace KittyEngine.Core.Client.Behaviors.Commands
{
    internal class SynchronizeBehavior : ClientBehavior
    {
        private ILogger _logger;

        public SynchronizeBehavior(ILogger logger)
        {
            _logger = logger;

        }
        public override void OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "sync")
            {
                return;
            }

            var entity = input.Args["entity"];
            var mode = input.Args["mode"];
            var value = input.Args["value"];

            if (mode == "full")
            {
                context.State.GameState = JsonConvert.DeserializeObject<GameState>(value);
                context.StateUpdated = true;
            }
            else if (mode == "patch")
            {
                var currentPlayer = context.State.GameState.GetPlayer(context.State.ConnectedUser.Guid);
                var lookDirection = currentPlayer.LookDirection;

                var jsonPatch = JsonConvert.DeserializeObject<JsonPatchDocument>(value);
                jsonPatch.ApplyTo(context.State.GameState);

                // HACK : Keep player's LookDirection because actually, it is refreshed only by the client
                currentPlayer.LookDirection = lookDirection;

                context.StateUpdated = true;
            }

            var player = context.State.GameState?.Players.Values.FirstOrDefault(x => x.Guid == context.State.ConnectedUser.Guid);
            if (player != null)
            {
                _logger.Log(LogLevel.Info, $"[Client] {player.PeerId} ({player.Name}) : Position: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
            }
            else
            {
                _logger.Log(LogLevel.Info, $"[Client] Connected. No position yet.");
            }
        }
    }
}
