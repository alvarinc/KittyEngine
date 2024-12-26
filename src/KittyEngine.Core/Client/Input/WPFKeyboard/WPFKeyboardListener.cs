using KittyEngine.Core.Client.Input.WPFKeyboard.Converters;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;
using System.Collections.Generic;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public class WPFKeyboardListener : IWPFKeyboardListener
    {
        private LightFactory<IKeyboardEventConverter> _commandFactory;
        private List<KeyboardInput>  _inputs = new List<KeyboardInput>();
        private object padlock = new object();

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

        public WPFKeyboardListener(IServiceContainer container, IKeyboadPressedKeyMap keyboadPressedKeyMap)
        {
            _keyboadPressedKeyMap = keyboadPressedKeyMap;
            IsEnabled = true;

            _commandFactory = new LightFactory<IKeyboardEventConverter>(container);
            _commandFactory.Add<WPFKeyboard.Converters.ExitConverter>("exit");
            _commandFactory.Add<WPFKeyboard.Converters.MoveConverter>("move");
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
                inputs.AddRange(_inputs.ToArray());
                _inputs.Clear();
            }

            foreach (var keyboardInput in inputs)
            {
                foreach (var key in _commandFactory.Keys)
                {
                    var cmd = _commandFactory
                        .Get(key)
                        .Convert(gameState, playerId, keyboardInput);

                    if (cmd != null)
                    {
                        results.Add(cmd);
                        break;
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
                _keyboadPressedKeyMap.RegisterKeyboardPressedKey(e.Key);

                _inputs.Add(new KeyboardInput
                {
                    Type = KeyboardInputType.KeyDown,
                    KeyDown = e.Key,
                    IsNewKeyDown = !previousPressedKeys.Contains(e.Key),
                    KeyUp = Key.None,
                    PressedKeys = _keyboadPressedKeyMap.GetPressedKeys()
                });
            }

            if (e.RoutedEvent.Name == "KeyUp")
            {
                _keyboadPressedKeyMap.RemoveKeyboardPressedKey(e.Key);

                _inputs.Add(new KeyboardInput
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
