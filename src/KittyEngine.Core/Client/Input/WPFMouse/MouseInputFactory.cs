using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public class MouseInputFactory : IMouseInputFactory
    {
        private IMouseControllerInterop _mouseControllerInterop;

        private Point? _mousePosition;

        public MouseInputFactory(IMouseControllerInterop mouseControllerInterop)
        {
            _mouseControllerInterop = mouseControllerInterop;
        }

        public MouseInput CreateMouseButtonInput(Viewport3D viewport, MouseButtonEventArgs e)
        {
            if (viewport == null)
            {
                return null;
            }

            var mousePosition = System.Windows.Input.Mouse.GetPosition(viewport);

            return CreateMouseClick(mousePosition, e.LeftButton, e.RightButton, e.ClickCount);
        }

        public MouseInput CreateMouseMoveInput(Viewport3D viewport, MouseEventArgs e)
        {
            if (viewport == null)
            {
                return null;
            }

            var firstPosition = !_mousePosition.HasValue;
            var previousMousePosition = _mousePosition;

            _mousePosition = System.Windows.Input.Mouse.GetPosition(viewport);

            if (firstPosition)
            {
                return null;
            }

            var dx = _mousePosition.Value.X - previousMousePosition.Value.X;
            var dy = _mousePosition.Value.Y - previousMousePosition.Value.Y;

            var mouseEvent = CreateMouseMove(_mousePosition.Value, e.LeftButton, e.RightButton, dx, dy);

            _mousePosition = _mouseControllerInterop.CenterCursorPosition(viewport);

            return mouseEvent;
        }

        public MouseInput CreateMouseWheelInput(Viewport3D viewport, MouseWheelEventArgs e)
        {
            if (viewport == null)
            {
                return null;
            }

            _mousePosition = System.Windows.Input.Mouse.GetPosition(viewport);

            return CreateMouseWheel(_mousePosition.Value, e.LeftButton, e.RightButton, e.Delta);
        }

        public void Reset()
        {
            _mousePosition = null;
        }

        private MouseInput CreateMouseClick(Point position, MouseButtonState leftButton, MouseButtonState rightButton, int clickCount)
        {
            return new MouseInput { Type = MouseInputType.Click, Position = position, LeftButton = leftButton, RightButton = rightButton };
        }

        private MouseInput CreateMouseMove(Point position, MouseButtonState leftButton, MouseButtonState rightButton, double dx, double dy)
        {
            return new MouseInput { Type = MouseInputType.Move, Position = position, LeftButton = leftButton, RightButton = rightButton, DX = dx, DY = dy };
        }

        private MouseInput CreateMouseWheel(Point position, MouseButtonState leftButton, MouseButtonState rightButton, double delta)
        {
            return new MouseInput { Type = MouseInputType.Wheel, Position = position, LeftButton = leftButton, RightButton = rightButton, Delta = delta };
        }
    }
}
