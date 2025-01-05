using KittyEngine.Core.Physics;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Client.Input.WPFKeyboard.Converters
{
    internal class JumpConverter : IKeyboardEventConverter
    {
        private IPrimitiveMoveService _primitiveMoveService;

        public JumpConverter(IPrimitiveMoveService primitiveMoveService)
        {
            _primitiveMoveService = primitiveMoveService;
        }

        public GameCommandInput Convert(GameState gameState, string playerId, KeyboardInput input)
        {
            var player = gameState.GetPlayer(playerId);
            if (player == null || !player.IsGrounded)
            {
                return null;
            }

            var pressedKeys = input.PressedKeys.AsEnumerable();
            var moveForward = pressedKeys.Contains(Key.Z);
            var moveBackward = pressedKeys.Contains(Key.S);
            var moveLeft = pressedKeys.Contains(Key.Q);
            var moveRight = pressedKeys.Contains(Key.D);

            var jump = pressedKeys.Contains(Key.Space);

            if (jump)
            {
                var identity = new Vector3D(0, 0, 0);

                var moveForwardVector = moveForward ? _primitiveMoveService.GetMoveLongitudinal(player, 1) : identity;
                var moveBackwardVector = moveBackward ? _primitiveMoveService.GetMoveLongitudinal(player, -1) : identity;
                var moveLeftVector = moveLeft ? _primitiveMoveService.GetMoveLateral(player, 1) : identity;
                var moveRightVector = moveRight ? _primitiveMoveService.GetMoveLateral(player, -1) : identity;

                var direction = moveForwardVector + moveBackwardVector + moveLeftVector + moveRightVector;// + moveAscendVector + moveDescendVector;

                if (direction != identity)
                {
                    direction.Normalize();
                }

                return new GameCommandInput("jump")
                        .AddArgument("direction", direction.ToString());
            }

            return null;
        }
    }
}
