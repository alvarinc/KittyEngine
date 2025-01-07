using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Physics;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Behaviors.Inputs
{
    internal class RotateBehavior : ClientBehavior
    {
        private IConfigurationService _configurationService;
        private IPrimitiveMoveService _primitiveMoveService;

        public RotateBehavior(IConfigurationService configurationService, IPrimitiveMoveService primitiveMoveService)
        {
            _configurationService = configurationService;
            _primitiveMoveService = primitiveMoveService;

        }

        public override GameCommandInput OnMouseEvent(GameState gameState, string playerId, MouseInput input)
        {
            if (input.Type != MouseInputType.Move)
            {
                return null;
            }

            var player = gameState.GetPlayer(playerId);
            if (player == null)
            {
                return null;
            }

            var body = new RigidBody(player);

            var moveSettings = _configurationService.GetMouseSettings();

            if (input.DX != 0)
            {
                body.LookDirection = _primitiveMoveService.GetHorizontalRotation(
                    body,
                    -input.DX * moveSettings.MouseRotationSpeed * (moveSettings.MouseXInverted ? -1 : 1));
            }

            if (input.DY != 0)
            {
                body.LookDirection = _primitiveMoveService.GetVerticalRotation(
                    body,
                    input.DY * moveSettings.MouseRotationSpeed * (moveSettings.MouseYInverted ? -1 : 1));
            }

            if (body.LookDirection != player.LookDirection)
            {
                player.LookDirection = body.LookDirection;

                return new GameCommandInput("rotate3d")
                    .AddArgument("direction", body.LookDirection.ToString());
            }

            return null;
        }
    }
}
