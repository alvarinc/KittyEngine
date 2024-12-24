using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard.Converters
{
    internal class ExitConverter : IKeyboardEventConverter
    {
        public GameCommandInput Convert(GameState gameState, string playerId, string keyPressed)
        {
            if (keyPressed == "Escape")
            {
                Console.WriteLine("[Client] ESC key pressed. Stopping client...");
                return new GameCommandInput("exit");
            }

            return null;
        }
    }
}
