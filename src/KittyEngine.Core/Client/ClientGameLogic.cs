﻿namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Commands;
    using KittyEngine.Core.Client.Input.Keyboard;
    using KittyEngine.Core.Client.Output;
    using KittyEngine.Core.Server;
    using KittyEngine.Core.State;

    public interface IClientGameLogic
    {
        void ViewAs(string playerId);

        List<GameCommandInput> HandleInputEvents();

        void HandleServerMessage(GameCommandInput input);

        void RenderOutput();
    }

    internal class ClientGameLogic : IClientGameLogic
    {
        private string _playerId;
        private GameState _gameState = null;
        private bool _gameStateUpdated = false;
        private KeyboardEventHandler _keyboardEventHandler;

        public ClientGameLogic()
        {
            _keyboardEventHandler = new KeyboardEventHandler();
        }

        public void ViewAs(string playerId)
        {
            _playerId = playerId;
        }

        public List<GameCommandInput> HandleInputEvents()
        {
            return _keyboardEventHandler.HandleEvents();
        }

        public void HandleServerMessage(GameCommandInput input)
        {
            IGameCommand cmd = null;
            if (input.Command == "sync")
            {
                cmd = new SynchronizeCommand();
            }

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

        public void RenderOutput()
        {
            if (_gameStateUpdated)
            {
                Renderer.Render(_gameState, _playerId);
                _gameStateUpdated = false;
            }
        }
    }
}