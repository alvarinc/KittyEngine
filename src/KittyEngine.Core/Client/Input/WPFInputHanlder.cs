using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    internal class WPFInputHanlder : IInputHandler
    {
        private IClientBehaviorContainer _behaviorContainer;

        private WPFKeyboardListener _keyboardListener;
        private WPFMouseListener _mouseListener;

        public WPFInputHanlder(IClientBehaviorContainer behaviorContainer)
        {
            _behaviorContainer = behaviorContainer;

            _keyboardListener = new WPFKeyboardListener();
            _mouseListener = new WPFMouseListener();
        }

        public bool IsEnabled => _keyboardListener.IsEnabled;

        public void Disable()
        {
            _keyboardListener.Disable();
            _mouseListener.Disable();
        }

        public void Enable()
        {
            _keyboardListener.Enable();
            _mouseListener.Enable();
        }

        public void Reset()
        {
            _keyboardListener.Reset();
            _mouseListener.Reset();
        }

        public void RegisterEvents(object gameHost)
        {
            var gameHostInterface = gameHost as IGameHost;
            if (gameHostInterface == null)
            {
                throw new ArgumentException("gameHost must be of type IGameHost");
            }

            _keyboardListener.RegisterKeyboardEvents(() => gameHostInterface.HostControl);
            _mouseListener.RegisterMouseEvents(() => gameHostInterface.HostControl, () => gameHostInterface.Viewport3D);
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();
            var clientBehaviors = _behaviorContainer.GetBehaviors();

            _keyboardListener.HandleEvents(input => 
            {
                var commands = clientBehaviors
                    .Select(behavior => behavior.OnKeyboardEvent(gameState, playerId, input))
                    .Where(command => command != null);

                results.AddRange(commands);
            });

            _mouseListener.HandleEvents(input =>
            {
                var commands = clientBehaviors
                    .Select(behavior => behavior.OnMouseEvent(gameState, playerId, input))
                    .Where(command => command != null);

                results.AddRange(commands);
            });

            return results;
        }
    }
}
