using KittyEngine.Core.Client.Model;
using System.Windows;

namespace KittyEngine.Core.Client.Outputs
{
    /// <summary>
    /// Interaction logic for ServerInputDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : Window
    {
        public ServerInput ServerInput { get; private set; }

        public PlayerInput PlayerInput { get; private set; }

        public ConnectionDialog()
        {
            InitializeComponent();

            var guid = Guid.NewGuid();
            PlayerInput = new PlayerInput(guid.ToString(), $"Player-{guid}");
            UsernameTextBox.Text = PlayerInput.Name;

            ServerInput = new ServerInput("localhost", 9050);
            ServerAddressTextBox.Text = ServerInput.Address;
            PortTextBox.Text = ServerInput.Port.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerInput = new PlayerInput(PlayerInput.Guid, UsernameTextBox.Text);

            if (int.TryParse(PortTextBox.Text, out int port))
            {
                ServerInput = new ServerInput(ServerAddressTextBox.Text, port);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid port number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
