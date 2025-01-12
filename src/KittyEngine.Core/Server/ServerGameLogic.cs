namespace KittyEngine.Core.Server
{
    using KittyEngine.Core.Physics;
    using KittyEngine.Core.Server.Behaviors;
    using KittyEngine.Core.Server.Commands;
    using KittyEngine.Core.Server.Model;
    using KittyEngine.Core.Services;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.State;
    using LiteNetLib;
    using System;
    using System.Collections.Generic;

    public interface IServerGameLogic
    {
        void Bind(NetworkAdapter networkAdapter);

        void OnClientConnected(NetPeer peer);

        void OnMessageReceived(NetPeer peer, GameCommandInput input);

        void OnStartGame();

        void GameLoop(GameTime gameTime);

        void OnStopGame();
    }

    internal class ServerGameLogic : IServerGameLogic
    {
        private class GameCommandRequest
        {
            public GameCommandRequest(int peerId, GameCommandInput input)
            {
                PeerId = peerId;
                Input = input;
            }

            public GameCommandInput Input { get; set; }
            public int PeerId { get; set; }
        }

        private enum PeerSynchronizationMode
        {
            None,
            Patch,
            Full
        }

        private static double _millisecondsPerUpdate = 10;

        private readonly ILogger _logger;
        private readonly IPhysicsEngine _physicsEngine;
        private readonly IServerBehaviorContainer _serverBehaviorContainer;
        private readonly ServerState _serverState;

        private NetworkAdapter _networkAdapter;
        private readonly Queue<GameCommandRequest> _gameCommmandRequests = new();

        public ServerGameLogic(ILogger logger, IPhysicsEngine physicsEngine, IServerBehaviorContainer serverBehaviorContainer, ServerState serverState)
        {
            _logger = logger;
            _physicsEngine = physicsEngine;
            _serverBehaviorContainer = serverBehaviorContainer;
            _serverState = serverState;
        }

        public void Bind(NetworkAdapter networkAdapter)
        {
            if (_networkAdapter != null)
            {
                throw new InvalidOperationException("A network adapter is already connected.");
            }

            _networkAdapter = networkAdapter;
        }

        public void OnClientConnected(NetPeer peer)
        {
            EnsureIsConnected();

            // Register a new player for the client
            _serverState.ConnectedUsers.Add(peer.Id, new Player(peer.Id));
            _logger.Log(LogLevel.Info, $"[Server] Player {peer.Id} : connected.");
        }

        public void OnMessageReceived(NetPeer peer, GameCommandInput input)
        {
            EnsureIsConnected();

            _gameCommmandRequests.Enqueue(new GameCommandRequest(peer != null ? peer.Id : -1, input));
        }

        public void OnStartGame()
        {
            var behaviors = _serverBehaviorContainer.GetBehaviors();
            foreach (var behavior in behaviors)
            {
                behavior.OnStartGame();
            }
        }

        public void GameLoop(GameTime gameTime)
        {
            EnsureIsConnected();

            // Get clients inputs
            _networkAdapter.HandlePeersEvents();

            var synchronizer = new StateSynchronizer<GameState>(_serverState.GameState);

            // Update players
            var commandResultByPeers = ExecuteCommands();

            // Update physics
            if (_physicsEngine.UpdatePhysics(_serverState.GameState, gameTime.DeltaTime.TotalMilliseconds))
            {
                StateUpdatedByServer(commandResultByPeers);
            }

            // Send updated state to clients
            SynchronizeClients(commandResultByPeers, synchronizer);

            WaitForNextFrame(gameTime);
        }

        public void OnStopGame()
        {
            var behaviors = _serverBehaviorContainer.GetBehaviors();
            foreach (var behavior in behaviors)
            {
                behavior.OnStopGame();
            }
        }

        private void WaitForNextFrame(GameTime gameTime)
        {
            var elapsed = gameTime.DeltaTime;
            gameTime.Mark();
            if (elapsed.TotalMilliseconds < _millisecondsPerUpdate)
            {
                System.Threading.Thread.Sleep((int)(_millisecondsPerUpdate - elapsed.TotalMilliseconds));
            }
            else if (elapsed.TotalMilliseconds > _millisecondsPerUpdate * 2)
            {
                _logger.Log(LogLevel.Warn, $"Server update took too long: {elapsed.TotalMilliseconds}ms");
            }
        }

        private void EnsureIsConnected()
        {
            if (_networkAdapter == null)
            {
                _logger.Log(LogLevel.Error, $"No network adapter connected.");
                throw new InvalidOperationException("No network adapter connected.");
            }
        }

        private Dictionary<int, GameCommandResult> ExecuteCommands()
        {
            var peerCommandResults = new Dictionary<int, GameCommandResult>();

            while (_gameCommmandRequests.Count > 0)
            {
                var request = _gameCommmandRequests.Dequeue();
                var result = ExecuteCommand(request);

                if (!peerCommandResults.TryGetValue(request.PeerId, out GameCommandResult currentResult))
                {
                    peerCommandResults[request.PeerId] = result;
                }
                else
                {
                    peerCommandResults[request.PeerId] = currentResult.Append(result);
                }
            }

            return peerCommandResults;
        }

        private GameCommandResult ExecuteCommand(GameCommandRequest request)
        {
            Player player = null;
            if (request.PeerId != -1)
            {
                if (!_serverState.ConnectedUsers.TryGetValue(request.PeerId, out player))
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {request.PeerId} : Received a message from an unknown player.");
                    return GameCommandResult.None;
                }

                if (request.Input.Command != "join" && string.IsNullOrEmpty(player.Guid))
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {request.PeerId} joined no games.");
                    return GameCommandResult.None;
                }
            }

            var synchronizer = new StateSynchronizer<GameState>(_serverState.GameState);

            var behaviors = _serverBehaviorContainer.GetBehaviors();
            var result = new GameCommandResult();
            foreach (var behavior in behaviors)
            {
                var commandResult = behavior.OnCommandReceived(new GameCommandContext(_networkAdapter, _serverState.GameState, player), request.Input);
                result = result.Append(commandResult);
            }

            return result;
        }

        private void StateUpdatedByServer(Dictionary<int, GameCommandResult> peerCommandResults)
        {
            peerCommandResults[-1] = new GameCommandResult { StateUpdated = true };
        }

        private void ApplyServerResults(Dictionary<int, GameCommandResult> peerCommandResults)
        {
            // Add the server command results to all clients
            if (peerCommandResults.ContainsKey(-1))
            {
                foreach (var peerId in peerCommandResults.Keys)
                {
                    if (peerId == -1)
                    {
                        continue;
                    }

                    peerCommandResults[peerId] = peerCommandResults[peerId].Append(peerCommandResults[-1]);
                }
            }
        }

        private void SynchronizeClients(Dictionary<int, GameCommandResult> commandResultByPeers, StateSynchronizer<GameState> synchronizer)
        {
            if (!commandResultByPeers.Values.Any(x => x.StateUpdated))
            {
                return;
            }

            var patchCmd = new GameCommandInput("sync")
                .AddArgument("entity", "gamestate")
                .AddArgument("mode", "patch")
                .AddArgument("value", synchronizer.GetJsonPatch());

            var fullCmd = new GameCommandInput("sync")
                .AddArgument("entity", "gamestate")
                .AddArgument("mode", "full")
                .AddArgument("value", synchronizer.GetJson());

            ApplyServerResults(commandResultByPeers);

            foreach (var connectedPeer in _networkAdapter.GetConnectedPeers())
            {
                var mode = PeerSynchronizationMode.Patch;

                if (commandResultByPeers.TryGetValue(connectedPeer.Id, out GameCommandResult commandResult))
                {
                    if (commandResult.PeerInitializated)
                    {
                        mode = PeerSynchronizationMode.Full;
                    }
                }

                _logger.Log(LogLevel.Info, $"[Server] Player {connectedPeer.Id} : {mode} synchronize");
                _networkAdapter.SendMessage(connectedPeer, mode == PeerSynchronizationMode.Full ? fullCmd : patchCmd);

                if (!_serverState.GameState.Players.ContainsKey(connectedPeer.Id))
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {connectedPeer.Id} : Disconnect");
                    connectedPeer.Disconnect();
                    _serverState.ConnectedUsers.Remove(connectedPeer.Id);
                }
            }
        }
    }
}
