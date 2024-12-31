using KittyEngine.Core;
using KittyEngine.Core.Client.Model;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.SampleMaps;
using System.Windows;

namespace KittyEngine.SampleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += (sender, e) => StartGame();
        }

        private void StartGame()
        {
            var guid = Guid.NewGuid();

            var dialog = new ConnectionDialog($"Player-{guid}", "localhost", 9050);
            if (dialog.ShowDialog() == true)
            {
                var server = new ServerInput(dialog.ServerAddress, dialog.ServerPort);
                var player = new PlayerInput(guid.ToString(), dialog.Username);

                Engine.StartWPFClient(player, server:server, placeholder: gameView, configure: container =>
                {
                    var contentService = container.Get<IContentService>();
                    contentService.RegisterContentFromSampleMaps();
                });
            }
            else
            {
                Close();
            }
        }
    }
}