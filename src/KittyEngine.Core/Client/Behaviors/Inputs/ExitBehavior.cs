using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Behaviors.Inputs
{
    public class ExitBehavior : ClientBehavior
    {
        ClientState _clientState;

        public ExitBehavior(ClientState clientState)
        {
            _clientState = clientState;
        }

        public override GameCommandInput OnKeyboardEvent(GameState gameState, string playerId, KeyboardInput input)
        {
            if (input.PressedKeys.Contains(System.Windows.Input.Key.Escape))
            {
                _clientState.Mode = ClientMode.InGameMenu;
                //return new GameCommandInput("exit");
            }

            return null;
        }
    }
}
