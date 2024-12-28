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
            var guid = Guid.NewGuid().ToString();
            var player = new PlayerInput(guid, $"Player-{guid}");

            Engine.StartWPFClient(player, placeholder: gameView, configure:container => 
            {
                var contentService = container.Get<IContentService>();
                contentService.RegisterContentFromSampleMaps();
            });
        }
    }
}