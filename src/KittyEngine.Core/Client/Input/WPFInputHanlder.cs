using KittyEngine.Core.Client.Input.ConsoleKeyboard;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    internal class WPFInputHanlder : IInputHandler
    {
        private IWPFKeyboardListener _keyboardListener;
        private IWPFMouseListener _mouseListener;
        public WPFInputHanlder(IWPFKeyboardListener keyboardListener, IWPFMouseListener mouseListener)
        {
            _keyboardListener = keyboardListener;
            _mouseListener = mouseListener;
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var result = new List<GameCommandInput>();

            result.AddRange(_keyboardListener.HandleEvents(gameState, playerId));
            result.AddRange(_mouseListener.HandleEvents(gameState, playerId));

            return result;
        }
    }
}
