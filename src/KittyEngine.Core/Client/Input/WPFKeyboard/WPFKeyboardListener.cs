using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public class WPFKeyboardListener
    {
        private Queue<KeyboardInput> _inputs = new Queue<KeyboardInput>();
        private object padlock = new object();

        private KeyboadPressedKeyMap _keyboadPressedKeyMap = new KeyboadPressedKeyMap();

        private bool _isEnabled = true;

        public bool IsEnabled => _isEnabled;

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Enable()
        {
            _isEnabled = true;
            Reset();
        }

        public void Reset()
        {
            _keyboadPressedKeyMap.Reset();
        }

        public void RegisterKeyboardEvents(Func<UserControl> hostControlAccessor)
        {
            hostControlAccessor().KeyDown += UserControl_KeyEvents;
            hostControlAccessor().KeyUp += UserControl_KeyEvents;
        }

        public void HandleEvents(Action<KeyboardInput> handler)
        {
            if (!IsEnabled)
            {
                return;
            }

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
                        PressedKeys = keyPressed
                    });
                }
            }

            // If no handler, still catch events and do nothing
            if (handler == null)
            {
                return;
            }

            // Send inputs to behaviors
            foreach (var keyboardInput in inputs)
            {
                handler(keyboardInput);
            }
        }

        private void UserControl_KeyEvents(object sender, KeyEventArgs e)
        {
            if (!IsEnabled)
            {
                return;
            }

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
