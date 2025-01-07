using KittyEngine.Core;
using KittyEngine.Core.Client.Outputs;
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
            var dialog = new ConnectionDialog();
            if (dialog.ShowDialog() == true)
            {
                Engine.StartWPFClient(dialog.PlayerInput, server:dialog.ServerInput, placeholder: gameView, onloadBehaviors: behaviors =>
                {
                    behaviors.Add(new RegisterSampleMapsBehavior());
                });
            }
            else
            {
                Close();
            }
        }
    }
}