using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public class MouseInput
    {
        public MouseInputType Type;
        public System.Windows.Point Position;
        public int ClickCount;
        public double DX;
        public double DY;
        public double Delta;
        public MouseButtonState LeftButton;
        public MouseButtonState RightButton;
    }
}
