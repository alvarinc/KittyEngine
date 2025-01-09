using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public class WPFMouseListener
    {
        private List<MouseInput> _inputs = new List<MouseInput>();
        private object padlock = new object();

        private MouseInputFactory _mouseInputFactory = new MouseInputFactory();
        private Func<UserControl> _hostControlAccessor;
        private Func<Viewport3D> _viewport3DAccesor;

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
            _mouseInputFactory.Reset();
        }

        public void RegisterMouseEvents(Func<UserControl> hostControlAccessor, Func<Viewport3D> viewport3DAccesor)
        {
            _hostControlAccessor = hostControlAccessor;
            _viewport3DAccesor = viewport3DAccesor;
            _hostControlAccessor().MouseEnter += UserControl_MouseEvents;
            _hostControlAccessor().MouseMove += UserControl_MouseEvents;
            _hostControlAccessor().MouseDown += UserControl_MouseButtonEvents;
            _hostControlAccessor().MouseUp += UserControl_MouseButtonEvents;
            _hostControlAccessor().MouseWheel += UserControl_MouseWheel;
        }

        public void HandleEvents(Action<MouseInput> handler)
        {
            if (!IsEnabled)
            {
                return;
            }

            var inputs = new List<MouseInput>();
            lock (padlock)
            {
                inputs.AddRange(_inputs.ToArray());
                _inputs.Clear();
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

        private void UserControl_MouseEvents(object sender, MouseEventArgs e)
        {
            //if (State.Shared.Mode != GameMode.Play)
            //{
            //    return;
            //}

            var mouseEvent = _mouseInputFactory.CreateMouseMoveInput(_viewport3DAccesor(), e);
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

            var mouseEvent = _mouseInputFactory.CreateMouseButtonInput(_viewport3DAccesor(), e);
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

            var mouseEvent = _mouseInputFactory.CreateMouseWheelInput(_viewport3DAccesor(), e);
            if (IsEnabled && mouseEvent != null)
            {
                _inputs.Add(mouseEvent);
            }
        }
    }
}
