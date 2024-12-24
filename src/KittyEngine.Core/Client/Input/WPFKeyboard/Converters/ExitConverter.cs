using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.WPFKeyboard.Converters
{
    internal class ExitConverter : IKeyboardEventConverter
    {
        public GameCommandInput Convert(GameState gameState, string playerId, KeyboardInput keyboardInput)
        {
            if (keyboardInput.PressedKeys.Contains(System.Windows.Input.Key.Escape)) 
            {
                return new GameCommandInput("exit");
            }

            return null;
        }
    }
}
