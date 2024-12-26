using KittyEngine.Core.Server;
using KittyEngine.Core.State;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard.Converters
{
    internal class MoveConverter : IKeyboardEventConverter
    {
        public GameCommandInput Convert(GameState gameState, string playerId, string keyPressed)
        {
            float dx = 0, dy = 0, dz = 0;

            // Map keys to movement
            switch (keyPressed)
            {
                case "UpArrow": dz = 1; break;  // Forward
                case "Z": dz = 1; break;  // Forward
                case "S": dz = -1; break; // Backward
                case "DownArrow": dz = -1; break; // Backward
                case "Q": dx = -1; break; // Left
                case "LeftArrow": dx = -1; break; // Left
                case "D": dx = 1; break;  // Right
                case "RightArrow": dx = 1; break;  // Right
                case "Spacebar": dy = 1; break; // Up
                case "C": dy = -1; break;     // Down
            }

            if (dx != 0 || dy != 0 || dz != 0)
            {
                // Send movement command to the server
                var cmd = new GameCommandInput("move");
                cmd.Args["direction"] = new Vector3D(dx, dy, dz).ToString();

                return cmd;
            }

            return null;
        }
    }
}
