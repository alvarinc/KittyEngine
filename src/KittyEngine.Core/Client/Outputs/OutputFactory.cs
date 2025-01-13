using KittyEngine.Core.State;
using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Outputs
{
    public class OutputFactory : IOutputFactory
    {
        private Func<Viewport3D> _viewportBuilder = () => new Viewport3D
        { 
            ClipToBounds = true, 
            Focusable = true, 
            Cursor = Cursors.None,
        };

        private Dictionary<OutputTypes, Func<UserControl>> _outputBuilders;

        private ClientState _clientState;

        public OutputFactory(ClientState clientState)
        {
            _clientState = clientState;
            _outputBuilders = new Dictionary<OutputTypes, Func<UserControl>>
            {
                { OutputTypes.StartupScreen, () => new StartupScreen(_clientState) },
                //{ OutputTypes.OutGameMenu, () => new Menus.OutGameMenu.MainMenu() },
                //{ OutputTypes.LoadScreen, () => new LoadScreen() },
                { OutputTypes.InGameMenu, () => new Menus.InGameMenu.MainMenu(_clientState) },
                //{ OutputTypes.HeadUpDisplay, () => new HeadUpDisplay() },
                { OutputTypes.ExitScreen, () => new ExitScreen() },
            };
        }

        public Viewport3D CreateViewport3D()
        {
            return _viewportBuilder();
        }

        public void RegisterViewport(Func<Viewport3D> builder)
        {
            _viewportBuilder = builder;
        }

        public UserControl CreateOutput(OutputTypes type)
        {
            if (_outputBuilders.ContainsKey(type))
            {
                return _outputBuilders[type]();
            }

            return null;
        }
    }
}
