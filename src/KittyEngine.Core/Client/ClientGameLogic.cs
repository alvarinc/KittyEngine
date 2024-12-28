namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Commands;
    using KittyEngine.Core.Client.Input;
    using KittyEngine.Core.Graphics;
    using KittyEngine.Core.Server;
    using KittyEngine.Core.Services.IoC;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.State;
    using System.Diagnostics;

    public interface IClientGameLogic
    {
        void Bind(NetworkAdapter networkAdapter);

        void HandleServerMessage(GameCommandInput input);

        void RenderLoop();

        void Terminate(CancellationToken token);
    }

    internal class ClientGameLogic : IClientGameLogic
    {
        private static double _millisecondsPerUpdate = 10;

        private bool _gameStateUpdated = false;

        private NetworkAdapter _networkAdapter;

        private ILogger _logger;
        private ILightFactory<IGameCommand> _commandFactory;
        private IRenderer _renderer;
        private IInputHandler _inputHandler;
        private ClientState _clientState;

        public ClientGameLogic(ILogger logger, ILightFactory<IGameCommand> commandFactory, IRenderer renderer, IInputHandler inputHandler, ClientState clientState)
        {
            _logger = logger;
            _commandFactory = commandFactory;
            _renderer = renderer;
            _inputHandler = inputHandler;
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

            var cmd = _commandFactory.Get(input.Command);

            if (cmd != null)
            {
                if (cmd.ValidateParameters(input))
                {
                    var context = new GameCommandContext(_networkAdapter) { State = _clientState };
                    cmd.Execute(context);

                    if (context.StateUpdated)
                    {
                        _clientState.GameState = context.State.GameState;
                        _gameStateUpdated |= context.StateUpdated;
                    }
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

        public void Terminate(CancellationToken token)
        {

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
                _renderer.Render(_clientState.GameState, _clientState.ConnectedUser.Guid);
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
