using System.Windows;
using System.Windows.Controls;

namespace KittyEngine.Core.Client.Outputs.Menus.OutGameMenu
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
