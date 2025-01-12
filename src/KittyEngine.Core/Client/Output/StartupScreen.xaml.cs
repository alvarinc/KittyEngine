using KittyEngine.Core.State;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KittyEngine.Core.Client.Outputs
{
    /// <summary>
    /// Interaction logic for StartupScreen.xaml
    /// </summary>
    public partial class StartupScreen : UserControl
    {
        private ClientState _clientState;

        public StartupScreen(ClientState clientState)
        {
            InitializeComponent();

            _clientState = clientState;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _clientState.Mode = ClientMode.OutGameMenu;
            ((DispatcherTimer)sender).Stop();
        }
    }
}
