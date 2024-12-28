using System.Windows;

namespace KittyEngine.Core.State
{
    public class ClientWindowState
    {
        public bool IsInFullScreenMode { get; set; }

        public WindowStyle NormalWindowsStyle { get; set; }
        public WindowState NormalWindowsState { get; set; }
        public bool NormalTopmost { get; set; }
    }
}
