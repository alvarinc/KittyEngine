using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KittyEngine.SampleGame
{
    /// <summary>
    /// Interaction logic for ServerInputDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : Window
    {
        public string Username { get; private set; }

        public string ServerAddress { get; private set; }

        public int ServerPort { get; private set; }

        public ConnectionDialog()
        {
            InitializeComponent();
        }

        public ConnectionDialog(string name, string address, int port)
        {
            InitializeComponent();
            UsernameTextBox.Text = name;
            ServerAddressTextBox.Text = address;
            PortTextBox.Text = port.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text;
            ServerAddress = ServerAddressTextBox.Text;

            if (int.TryParse(PortTextBox.Text, out int port))
            {
                ServerPort = port;
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
