using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.Client.Input.WPFKeyboard.Converters;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public class WPFKeyboardListener : IWPFKeyboardListener
    {
        private LightFactory<IKeyboardEventConverter> _commandFactory;
        private Queue<KeyboardInput> _inputs = new Queue<KeyboardInput>();
        private object padlock = new object();

        private IClientBehaviorContainer _behaviorContainer;
        private IKeyboadPressedKeyMap _keyboadPressedKeyMap;

        private bool _isEnabled;

        public bool IsEnabled 
        {
            get { return _isEnabled; }
            set 
            {
                _keyboadPressedKeyMap.Reset();
                _isEnabled = value;
            }
        }

        public WPFKeyboardListener(IServiceContainer container, IKeyboadPressedKeyMap keyboadPressedKeyMap, IClientBehaviorContainer behaviorContainer)
        {
            _behaviorContainer = behaviorContainer;
            _keyboadPressedKeyMap = keyboadPressedKeyMap;
            IsEnabled = true;

            _commandFactory = new LightFactory<IKeyboardEventConverter>(container);
            _commandFactory.Add<WPFKeyboard.Converters.ExitConverter>("exit");
            _commandFactory.Add<WPFKeyboard.Converters.MoveConverter>("move");
            _commandFactory.Add<WPFKeyboard.Converters.JumpConverter>("jump");
        }

        public void RegisterKeyboardEvents(IGameHost host)
        {
            host.HostControl.KeyDown += UserControl_KeyEvents;
            host.HostControl.KeyUp += UserControl_KeyEvents;
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();
            
            var inputs = new List<KeyboardInput>();
            lock (padlock)
            {
                while (_inputs.Count > 0)
                {
                    inputs.Add(_inputs.Dequeue());
                }
            }

            // If no keyboard inputs, and still have pressed keys, send the PressedKeys map
            if (inputs.Count == 0)
            {
                var keyPressed = _keyboadPressedKeyMap.GetPressedKeys();
                if (keyPressed.Length > 0)
                {
                    inputs.Add(new KeyboardInput
                    {
                        Type = KeyboardInputType.KeyPressedMap,
                        PressedKeys = _keyboadPressedKeyMap.GetPressedKeys()
                    });
                }
            }

            var clientBehaviors = _behaviorContainer.GetBehaviors();
            foreach (var keyboardInput in inputs)
            {
                foreach (var clientBehavior in clientBehaviors)
                {
                    var cmd = clientBehavior.OnKeyboardEvent(gameState, playerId, keyboardInput);

                    if (cmd != null)
                    {
                        results.Add(cmd);
                    }
                }
            }

            return results;
        }

        private void UserControl_KeyEvents(object sender, KeyEventArgs e)
        {
            //if (State.Shared.Mode != GameMode.Play || !IsEnabled)
            //{
            //    return;
            //}

            if (e.RoutedEvent.Name == "KeyDown")
            {
                var previousPressedKeys = _keyboadPressedKeyMap.GetPressedKeys();
                var isNewKeyDown = !previousPressedKeys.Contains(e.Key);
                _keyboadPressedKeyMap.RegisterKeyboardPressedKey(e.Key);

                _inputs.Enqueue(new KeyboardInput
                {
                    Type = KeyboardInputType.KeyDown,
                    KeyDown = e.Key,
                    IsNewKeyDown = isNewKeyDown,
                    KeyUp = Key.None,
                    PressedKeys = _keyboadPressedKeyMap.GetPressedKeys()
                });
            }

            if (e.RoutedEvent.Name == "KeyUp")
            {
                _keyboadPressedKeyMap.RemoveKeyboardPressedKey(e.Key);

                _inputs.Enqueue(new KeyboardInput
                {
                    Type = KeyboardInputType.KeyUp,
                    KeyDown = Key.None,
                    IsNewKeyDown = false,
                    KeyUp = e.Key,
                    PressedKeys = _keyboadPressedKeyMap.GetPressedKeys()
                });
            }
        }
    }
}
