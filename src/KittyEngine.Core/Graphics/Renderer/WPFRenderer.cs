using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal class WPFRenderer : IRenderer
    {
        private IGameHost _host;
        private bool _initialized = false;

        private IMapRenderer _mapRenderer;

        public WPFRenderer(IMapRenderer mapRenderer)
        {
            _mapRenderer = mapRenderer;
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
                    _mapRenderer.BindGraphicsToViewport(_host);
                    _mapRenderer.LoadMap(_gameState.Map);
                });

                _initialized = true;
            }

            _host.Dispatcher.Invoke(() =>
            {
                _mapRenderer.UpdateCamera();
                _mapRenderer.UpdatePlayers();
            });
        }
    }
}
