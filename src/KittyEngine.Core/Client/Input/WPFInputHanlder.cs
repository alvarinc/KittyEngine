using KittyEngine.Core.Client.Input.ConsoleKeyboard;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    internal class WPFInputHanlder : IInputHandler
    {
        private IWPFKeyboardListener _keyboardListener;
        public WPFInputHanlder(IWPFKeyboardListener keyboardListener)
        {
            _keyboardListener = keyboardListener;
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            return _keyboardListener.HandleEvents(gameState, playerId);
        }
    }
}
