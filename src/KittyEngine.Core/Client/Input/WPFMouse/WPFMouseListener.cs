using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public class WPFMouseListener : IWPFMouseListener
    {
        private List<MouseInput> _inputs = new List<MouseInput>();
        private object padlock = new object();

        private IClientBehaviorContainer _behaviorContainer;
        private IMouseInputFactory _mouseInputFactory;
        private IGameHost _gameHost;

        public WPFMouseListener(IMouseInputFactory mouseInputFactory, IClientBehaviorContainer behaviorContainer)
        {
            _behaviorContainer = behaviorContainer;
            _mouseInputFactory = mouseInputFactory;
            IsEnabled = true;
        }

        public void RegisterMouseEvents(IGameHost gameHost)
        {
            _gameHost = gameHost;
            _gameHost.HostControl.MouseEnter += UserControl_MouseEvents;
            _gameHost.HostControl.MouseMove += UserControl_MouseEvents;
            _gameHost.HostControl.MouseDown += UserControl_MouseButtonEvents;
            _gameHost.HostControl.MouseUp += UserControl_MouseButtonEvents;
            _gameHost.HostControl.MouseWheel += UserControl_MouseWheel;
        }

        public bool IsEnabled { get; set; } = true;

        public void Reset()
        {
            _mouseInputFactory.Reset();
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();

            var inputs = new List<MouseInput>();
            lock (padlock)
            {
                inputs.AddRange(_inputs.ToArray());
                _inputs.Clear();
            }

            var clientBehaviors = _behaviorContainer.GetBehaviors();
            foreach (var mouseInput in inputs)
            {
                foreach (var behavior in clientBehaviors)
                {
                    var cmd = behavior.OnMouseEvent(gameState, playerId, mouseInput);

                    if (cmd != null)
                    {
                        results.Add(cmd);
                        break;
                    }
                }
            }

            return results;
        }

        private void UserControl_MouseEvents(object sender, MouseEventArgs e)
        {
            //if (State.Shared.Mode != GameMode.Play)
            //{
            //    return;
            //}

            var mouseEvent = _mouseInputFactory.CreateMouseMoveInput(_gameHost.Viewport3D, e);
            if (IsEnabled && mouseEvent != null)
            {
                _inputs.Add(mouseEvent);
            }
        }

        private void UserControl_MouseButtonEvents(object sender, MouseButtonEventArgs e)
        {
            //if (State.Shared.Mode != GameMode.Play)
            //{
            //    return;
            //}

            var mouseEvent = _mouseInputFactory.CreateMouseButtonInput(_gameHost.Viewport3D, e);
            if (IsEnabled && mouseEvent != null)
            {
                _inputs.Add(mouseEvent);
            }
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (State.Shared.Mode != GameMode.Play)
            //{
            //    return;
            //}

            var mouseEvent = _mouseInputFactory.CreateMouseWheelInput(_gameHost.Viewport3D, e);
            if (IsEnabled && mouseEvent != null)
            {
                _inputs.Add(mouseEvent);
            }
        }
    }
}
