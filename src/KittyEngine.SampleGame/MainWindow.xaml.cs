using KittyEngine.Core;
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
            if (Engine.ShowLoginDialog(out var loginResult) == true)
            {
                Engine.StartWPFClient(loginResult.PlayerInput, server: loginResult.ServerInput, placeholder: gameView, onloadBehaviors: behaviors =>
                {
                    behaviors.AddComposer(new RegisterSampleMapsBehavior(EngineRuntime.Client));
                });
            }
            else
            {
                Close();
            }
        }
    }
}