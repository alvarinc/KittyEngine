using KittyEngine.Core.State;
using System.Windows.Controls;

namespace KittyEngine.Core.Client
{
    /// <summary>
    /// Interaction logic for ExitScreen.xaml
    /// </summary>
    public partial class ExitScreen : UserControl
    {
        private ClientState _clientState;

        public ExitScreen(ClientState clientState)
        {
            InitializeComponent();
            this.Loaded += ExitScreen_Loaded;

            _clientState = clientState;
        }

        private void ExitScreen_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _clientState.Mode = ClientMode.Terminated;
        }
    }
}
