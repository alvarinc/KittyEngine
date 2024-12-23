namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Commands;
    using KittyEngine.Core.Client.Input.Keyboard;
    using KittyEngine.Core.Client.Output;
    using KittyEngine.Core.Server;
    using KittyEngine.Core.Services.IoC;
    using KittyEngine.Core.State;

    public interface IClientGameLogic
    {
        void ViewAs(string playerId);

        void Bind(NetworkAdapter networkAdapter);

        void HandleServerMessage(GameCommandInput input);

        void RenderLoop();
    }

    internal class ClientGameLogic : IClientGameLogic
    {
        private NetworkAdapter _networkAdapter;
        private string _playerId;
        private GameState _gameState = null;
        private bool _gameStateUpdated = false;
        private KeyboardEventHandler _keyboardEventHandler;
        private ILightFactory<IGameCommand> _commandFactory;

        public ClientGameLogic(ILightFactory<IGameCommand> commandFactory)
        {
            _commandFactory = commandFactory;
            _keyboardEventHandler = new KeyboardEventHandler();
        }

        public void ViewAs(string playerId)
        {
            _playerId = playerId;
        }

        public void Bind(NetworkAdapter networkAdapter)
        {
            if (_networkAdapter != null)
            {
                throw new InvalidOperationException("A network adapter is already connected.");
            }

            _networkAdapter = networkAdapter;
        }

        public void RenderLoop()
        {
            var inputs = HandleInputEvents();
            foreach (var input in inputs)
            {
                _networkAdapter.SendMessage(input);
            }

            _networkAdapter.HandleServerEvents();

            RenderOutput();

            Thread.Sleep(15);
        }

        public void HandleServerMessage(GameCommandInput input)
        {
            var cmd = _commandFactory.Create(input.Command);

            if (cmd != null)
            {
                if (cmd.ValidateParameters(input))
                {
                    var context = new GameCommandContext { GameState = _gameState, PlayerId = _playerId };
                    cmd.Execute(context);

                    if (context.StateUpdated)
                    {
                        _gameState = context.GameState;
                        _gameStateUpdated |= context.StateUpdated;
                    }
                }
            }
        }

        private List<GameCommandInput> HandleInputEvents()
        {
            return _keyboardEventHandler.HandleEvents();
        }

        private void RenderOutput()
        {
            if (_gameStateUpdated)
            {
                Renderer.Render(_gameState, _playerId);
                _gameStateUpdated = false;
            }
        }
    }
}
