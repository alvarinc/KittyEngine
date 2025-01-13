using KittyEngine.Core.State;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KittyEngine.Core.Client.Outputs.Menus.InGameMenu
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, IImageMenu
    {
        private ClientState _clientState;

        public MainMenu(ClientState clientState)
        {
            InitializeComponent();

            _clientState = clientState;
            Focusable = true;
        }

        public ImageSource InGameScreenShot { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {           
            imgScreenshoot.Source = InGameScreenShot;
            Focus();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            _clientState.Mode = ClientMode.InGame;
        }

        private void btnBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            _clientState.Mode = ClientMode.Exit;
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                _clientState.Mode = ClientMode.InGame;
            }
        }
    }
}
