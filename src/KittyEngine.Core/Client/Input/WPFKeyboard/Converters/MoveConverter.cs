using KittyEngine.Core.Physics;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Client.Input.WPFKeyboard.Converters
{
    internal class MoveConverter : IKeyboardEventConverter
    {
        private IPrimitiveMoveService _primitiveMoveService;

        public MoveConverter(IPrimitiveMoveService primitiveMoveService)
        {
            _primitiveMoveService = primitiveMoveService;
        }

        public GameCommandInput Convert(GameState gameState, string playerId, KeyboardInput keyboardInput)
        {
            var player = gameState.GetPlayer(playerId);
            if (player == null)
            {
                return null;
            }

            var pressedKeys = keyboardInput.PressedKeys.AsEnumerable();
            var moveForward = pressedKeys.Contains(Key.Z);
            var moveBackward = pressedKeys.Contains(Key.S);
            var moveLeft = pressedKeys.Contains(Key.Q);
            var moveRight = pressedKeys.Contains(Key.D);

            if (moveForward || moveBackward || moveLeft || moveRight)
            {
                var identity = new Vector3D(0, 0, 0);

                var moveForwardVector = moveForward ? _primitiveMoveService.GetMoveLongitudinal(player, 1) : identity;
                var moveBackwardVector = moveBackward ? _primitiveMoveService.GetMoveLongitudinal(player, -1) : identity;
                var moveLeftVector = moveLeft ? _primitiveMoveService.GetMoveLateral(player, 1) : identity;
                var moveRightVector = moveRight ? _primitiveMoveService.GetMoveLateral(player, -1) : identity;

                var direction = moveForwardVector + moveBackwardVector + moveLeftVector + moveRightVector;

                if (direction != identity)
                {
                    direction.Normalize();
                    var cmd = new GameCommandInput("move3d");
                    cmd.Args["direction"] = direction.ToString();
                    return cmd;
                }
            }

            return null;
        }
    }
}
