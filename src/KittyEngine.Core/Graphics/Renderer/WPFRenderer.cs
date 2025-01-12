using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal class WPFRenderer : IRenderer
    {
        private IGameHost _host;
        private bool _initialized = false;

        private IGameWorldRenderer _gameWorldRenderer;

        public WPFRenderer(IGameWorldRenderer gameWorldRenderer)
        {
            _gameWorldRenderer = gameWorldRenderer;
        }

        public void RegisterOutput(IGameHost host)
        {
            _host = host;
            _gameWorldRenderer.RegisterOutput(_host);
        }

        public void RenderFrame(GameState _gameState, string playerId)
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
                    _gameWorldRenderer.LoadGameWorld(_gameState.Map);
                });

                _initialized = true;
            }

            _host.Dispatcher.Invoke(() =>
            {
                _gameWorldRenderer.UpdateCamera();
                _gameWorldRenderer.UpdatePlayers();
            });
        }
    }
}
