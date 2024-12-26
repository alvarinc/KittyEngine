using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics.WPFRenderer
{
    internal class WPFRenderer : IRenderer
    {
        private IGameHost _host;
        private bool _initialized = false;

        private IOutputFactory _outputFactory;
        private WorldLoader _worldLoader;

        public WPFRenderer(IOutputFactory outputFactory, WorldLoader worldLoader)
        {
            _worldLoader = worldLoader;
            _outputFactory = outputFactory;
        }

        public void RegisterGraphicOutput(IGameHost host)
        {
            _host = host;
        }

        public void Render(GameState _gameState, string playerId)
        {
            var player = _gameState.GetPlayer(playerId);
            if (player == null)
            {
                return;
            }

            if (!_initialized)
            {
                _host.Dispatcher.Invoke(() =>
                {
                    _worldLoader.BindGraphicsToViewport(_host);
                    //_host.AttachViewport(_outputFactory.CreateViewport3D());
                });

                _initialized = true;
            }

            _host.Dispatcher.Invoke(() =>
            {
                _worldLoader.UpdateCamera(player);
            });
            // TODO: Implement rendering logic here.
        }
    }
}
