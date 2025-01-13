using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Graphics;
using KittyEngine.Core.State;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KittyEngine.Core.Client.Outputs
{
    /// <summary>
    /// Interaction logic for GameHost.xaml
    /// </summary>
    public partial class GameHost : UserControl, IGameHost
    {
        private ClientState _clientState;

        private IOutputFactory _outputFactory;

        private ClientMode? _previousClientMode;

        private IInputHandler _inputHandler;

        private IRenderer _renderer;

        private ISynchronizableOutput _hud;

        public Viewport3D Viewport3D 
        { 
            get 
            { 
                if (gameViewportHost.Children.Count > 0)
                {
                    return gameViewportHost.Children[0] as Viewport3D;
                }

                return null;
            } 
        }

        public UserControl HostControl => this;

        public GameHost(IOutputFactory outputFactory, IInputHandler inputHandler, IRenderer renderer, ClientState clientState)
        {
            InitializeComponent();

            _outputFactory = outputFactory;
            gameViewportHost.Children.Clear();
            gameViewportHost.Children.Add(_outputFactory.CreateViewport3D());

            _inputHandler = inputHandler;
            _inputHandler.RegisterEvents(this);

            _renderer = renderer;
            _renderer.RegisterOutput(this);

            _clientState = clientState;

            Focusable = true;

            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = "UIThread";
            }

            Loaded += (s, e) =>
            {
                Focus();
            };
        }

        public void Synchronize()
        {
            if (_previousClientMode.HasValue && _previousClientMode == _clientState.Mode)
            {
                if (_clientState.Mode == ClientMode.ServerLoading || _clientState.Mode == ClientMode.ServerLoaded)
                {
                    var synchronizable = menuHost.Children[0] as ISynchronizableOutput;
                    if (synchronizable != null)
                    {
                        synchronizable.Synchronize();
                    }
                }

                return;
            }

            ImageSource inGameScreenshot = null;
            if (_clientState.Mode == ClientMode.InGameMenu)
            {
                inGameScreenshot = TakeScreenshot();
            }

            menuHost.Visibility = Visibility.Collapsed;
            gameViewportHost.Visibility = Visibility.Collapsed;
            gameHeadUpDisplayHost.Visibility = Visibility.Collapsed;

            switch (_clientState.Mode)
            {
                case ClientMode.Startup:
                    _hud = null;
                    menuHost.Children.Clear();
                    menuHost.Children.Add(_outputFactory.CreateOutput(OutputTypes.StartupScreen));
                    menuHost.Visibility = Visibility.Visible;
                    break;
                //    case GameMode.OutGameMenu:
                //        _hud = null;
                //        menuHost.Children.Clear();
                //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.OutGameMenu));
                //        menuHost.Visibility = Visibility.Visible;
                //        break;
                //    case GameMode.ServerLoading:
                //    case GameMode.ServerLoaded:
                //        if (_previousGameMode == GameMode.ServerLoading || _previousGameMode == GameMode.ServerLoaded)
                //        {
                //            menuHost.Visibility = Visibility.Visible;
                //            var synchronizable = menuHost.Children[0] as ISynchronizableOutput;
                //            if (synchronizable != null)
                //            {
                //                synchronizable.Synchronize();
                //            }

                //            break;
                //        }

                //        _hud = null;
                //        menuHost.Children.Clear();
                //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.LoadScreen));
                //        menuHost.Visibility = Visibility.Visible;
                //        break;
                case ClientMode.InGame:
                    menuHost.Children.Clear();
                    gameViewportHost.Visibility = Visibility.Visible;

                    gameHeadUpDisplayHost.Children.Clear();
                    _hud = null;

                    //if (State.HUD.IsEnabled)
                    //{
                    //    var hudControl = OutputFactory.CreateOutput(OutputTypes.HeadUpDisplay);
                    //    _hud = hudControl as ISynchronizableOutput;
                    //    gameHeadUpDisplayHost.Children.Add(hudControl);
                    //    gameHeadUpDisplayHost.Visibility = Visibility.Visible;
                    //}
                    //Focus();
                    break;
                case ClientMode.InGameMenu:
                    menuHost.Children.Clear();
                    var inGameMenu = _outputFactory.CreateOutput(OutputTypes.InGameMenu);
                    var imageMenu = inGameMenu as Menus.InGameMenu.IImageMenu;
                    if (imageMenu != null)
                    {
                        imageMenu.InGameScreenShot = inGameScreenshot;
                    }

                    menuHost.Children.Add(inGameMenu);
                    menuHost.Visibility = Visibility.Visible;
                    break;
                case ClientMode.Exit:
                    _hud = null;
                    menuHost.Children.Clear();
                    menuHost.Children.Add(_outputFactory.CreateOutput(OutputTypes.ExitScreen));
                    break;
            }

            if (_clientState.Mode == ClientMode.InGame)
            {
                _inputHandler.Enable();
                _inputHandler.Reset();
            }
            else
            {
                _inputHandler.Disable();
            }

            //State.HUD.Control = _hud;
            _previousClientMode = _clientState.Mode;
            Focus();
        }

        private ImageSource TakeScreenshot()
        {
            var screenshoot = new RenderTargetBitmap(
               (int)canvas.ActualWidth,
               (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            screenshoot.Render(canvas);

            return screenshoot;
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F11)
            {
                var inputHandlerEnabled = _inputHandler.IsEnabled;
                if (inputHandlerEnabled)
                {
                    _inputHandler.Disable();
                }

                var parentWindow = Window.GetWindow(this);
                if (_clientState.ClientWindow.IsInFullScreenMode)
                {
                    parentWindow.WindowStyle = _clientState.ClientWindow.NormalWindowsStyle;
                    parentWindow.Topmost = _clientState.ClientWindow.NormalTopmost;
                    parentWindow.WindowState = _clientState.ClientWindow.NormalWindowsState;
                    _clientState.ClientWindow.IsInFullScreenMode = false;
                }
                else
                {
                    _clientState.ClientWindow.NormalWindowsStyle = parentWindow.WindowStyle;
                    _clientState.ClientWindow.NormalWindowsState = parentWindow.WindowState;
                    _clientState.ClientWindow.NormalTopmost = parentWindow.Topmost;
                    parentWindow.WindowStyle = WindowStyle.None;
                    parentWindow.Topmost = true;
                    parentWindow.WindowState = WindowState.Normal;
                    parentWindow.WindowState = WindowState.Maximized;
                    _clientState.ClientWindow.IsInFullScreenMode = true;
                }

                if (inputHandlerEnabled)
                {
                    _inputHandler.Enable();
                }
            }
            //else if (e.Key == System.Windows.Input.Key.F12)
            //{
            //    if (terminalHost.Children.Count > 0)
            //    {
            //        terminalHost.Children.Clear();
            //        Focus();
            //        KeyboardListener.IsEnabled = true;
            //    }
            //    else if (State.Shared.Terminal.IsEnabled)
            //    {
            //        KeyboardListener.IsEnabled = false;
            //        var control = new TerminalControl();
            //        control.Height = ActualHeight / 2 - 10;
            //        terminalHost.Children.Add(control);
            //    }
            //}
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (terminalHost.Children.Count > 0)
            //{
            //    var control = terminalHost.Children[0] as TerminalControl;
            //    control.Height = ActualHeight / 2 - 10;
            //}
        }
    }
}
