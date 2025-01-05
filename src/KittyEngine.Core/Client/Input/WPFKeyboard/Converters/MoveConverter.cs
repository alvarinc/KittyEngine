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

            //var moveAscend = pressedKeys.Contains(Key.Space);
            //var moveDescend = pressedKeys.Contains(Key.LeftShift);

            if (moveForward || moveBackward || moveLeft || moveRight)// || moveAscend || moveDescend)
            {
                var zeroVector = new Vector3D(0, 0, 0);

                var moveForwardVector = moveForward ? _primitiveMoveService.GetMoveLongitudinal(player, 1) : zeroVector;
                var moveBackwardVector = moveBackward ? _primitiveMoveService.GetMoveLongitudinal(player, -1) : zeroVector;
                var moveLeftVector = moveLeft ? _primitiveMoveService.GetMoveLateral(player, 1) : zeroVector;
                var moveRightVector = moveRight ? _primitiveMoveService.GetMoveLateral(player, -1) : zeroVector;

                //var moveAscendVector = moveAscend ? player.UpDirection : identity;
                //var moveDescendVector = moveDescend ? -player.UpDirection : identity;

                var direction = moveForwardVector + moveBackwardVector + moveLeftVector + moveRightVector;// + moveAscendVector + moveDescendVector;

                if (direction != zeroVector)
                {
                    direction.Normalize();
                    
                    return new GameCommandInput("move3d")
                        .AddArgument("direction", direction.ToString());
                }
            }

            return null;
        }
    }
}
