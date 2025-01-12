namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Behaviors;
    using KittyEngine.Core.Client.Commands;
    using KittyEngine.Core.Client.Input;
    using KittyEngine.Core.Graphics;
    using KittyEngine.Core.Server;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.State;
    using System.Diagnostics;

    public interface IClientGameLogic
    {
        void Bind(NetworkAdapter networkAdapter);

        void HandleServerMessage(GameCommandInput input);

        void RenderLoop();
    }

    internal class ClientGameLogic : IClientGameLogic
    {
        private static double _millisecondsPerUpdate = 10;

        private bool _gameStateUpdated = false;

        private NetworkAdapter _networkAdapter;

        private ILogger _logger;
        private IRenderer _renderer;
        private IInputHandler _inputHandler;
        private IClientBehaviorContainer _behaviorContainer;
        private ClientState _clientState;

        public ClientGameLogic(ILogger logger, IRenderer renderer, IInputHandler inputHandler, ClientState clientState, IClientBehaviorContainer behaviorContainer)
        {
            _logger = logger;
            _renderer = renderer;
            _inputHandler = inputHandler;
            _behaviorContainer = behaviorContainer;
            _clientState = clientState;
        }

        public void Bind(NetworkAdapter networkAdapter)
        {
            if (_networkAdapter != null)
            {
                throw new InvalidOperationException("A network adapter is already connected.");
            }

            _networkAdapter = networkAdapter;
        }

        public void HandleServerMessage(GameCommandInput input)
        {
            EnsureIsConnected();

            var behaviors = _behaviorContainer.GetBehaviors();

            foreach (var behavior in behaviors)
            {
                var context = new GameCommandContext(_networkAdapter) { State = _clientState };
                behavior.OnCommandReceived(context, input);

                if (context.StateUpdated)
                {
                    _clientState.GameState = context.State.GameState;
                    _gameStateUpdated |= context.StateUpdated;
                }
            }
        }

        public void RenderLoop()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            EnsureIsConnected();

            var inputs = HandleInputEvents();
            foreach (var input in inputs)
            {
                _networkAdapter.SendMessage(input);
            }

            _networkAdapter.HandleServerEvents();

            RenderOutput();

            WaitForNextFrame(stopwatch);
        }

        private void WaitForNextFrame(Stopwatch stopwatch)
        {
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < _millisecondsPerUpdate)
            {
                System.Threading.Thread.Sleep((int)(_millisecondsPerUpdate - stopwatch.ElapsedMilliseconds));
            }
            else
            {
                _logger.Log(LogLevel.Warn, $"Client update took too long: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        private List<GameCommandInput> HandleInputEvents()
        {
            if (_clientState.ConnectedUser == null)
            {
                return new List<GameCommandInput>();
            }

            return _inputHandler.HandleEvents(_clientState.GameState, _clientState.ConnectedUser.Guid);
        }

        private void RenderOutput()
        {
            if (_clientState.ConnectedUser != null && _gameStateUpdated)
            {
                _renderer.RenderFrame(_clientState.GameState, _clientState.ConnectedUser.Guid);
                _gameStateUpdated = false;
            }
        }

        private void EnsureIsConnected()
        {
            if (_networkAdapter == null)
            {
                throw new InvalidOperationException("No network adapter connected.");
            }
        }
    }
}
