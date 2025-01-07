using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Behaviors.Inputs
{
    public class ExitBehavior : ClientBehavior
    {
        public override GameCommandInput OnKeyboardEvent(GameState gameState, string playerId, KeyboardInput input)
        {
            if (input.PressedKeys.Contains(System.Windows.Input.Key.Escape))
            {
                return new GameCommandInput("exit");
            }

            return null;
        }
    }
}
