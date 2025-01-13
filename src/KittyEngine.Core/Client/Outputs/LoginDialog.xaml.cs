using KittyEngine.Core.Client.Model;
using System.Windows;

namespace KittyEngine.Core.Client.Outputs
{
    /// <summary>
    /// Interaction logic for ServerInputDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginResult LoginResult { get; private set; }

        public LoginDialog()
        {
            InitializeComponent();

            LoginResult = LoginResult.GetDefault();
            UsernameTextBox.Text = LoginResult.PlayerInput.Name;
            ServerAddressTextBox.Text = LoginResult.ServerInput.Address;
            PortTextBox.Text = LoginResult.ServerInput.Port.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            LoginResult.PlayerInput = new PlayerInput(LoginResult.PlayerInput.Guid, UsernameTextBox.Text);

            if (int.TryParse(PortTextBox.Text, out int port))
            {
                LoginResult.ServerInput = new ServerInput(ServerAddressTextBox.Text, port);
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
