namespace KittyEngine.Core.Server
{
    using KittyEngine.Core.Physics;
    using KittyEngine.Core.Server.Commands;
    using KittyEngine.Core.Server.Model;
    using KittyEngine.Core.Services.IoC;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.State;
    using LiteNetLib;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public interface IServerGameLogic
    {
        void Bind(NetworkAdapter networkAdapter);

        void OnClientConnected(NetPeer peer);

        void OnMessageReceived(NetPeer peer, GameCommandInput input);

        void GameLoop();

        void Terminate(CancellationToken token);
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
        private readonly ILightFactory<IGameCommand> _commandFactory;
        private readonly IPhysicsEngine _physicsEngine;
        private readonly ServerState _serverState;

        private NetworkAdapter _networkAdapter;
        private readonly Queue<GameCommandRequest> _gameCommmandRequests = new();

        public ServerGameLogic(ILogger logger, ILightFactory<IGameCommand> commandFactory, IPhysicsEngine physicsEngine, ServerState serverState)
        {
            _logger = logger;
            _commandFactory = commandFactory;
            _physicsEngine = physicsEngine;
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

        public void GameLoop()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            EnsureIsConnected();

            // Get clients inputs
            _networkAdapter.HandlePeersEvents();

            var synchronizer = new StateSynchronizer<GameState>(_serverState.GameState);

            // Update players
            var commandResultByPeers = ExecuteCommands();

            // Update physics
            //_serverState.GameTime.Mark();
            _physicsEngine.UpdatePhysics(_serverState.GameState, stopwatch.ElapsedMilliseconds);

            // Send updated state to clients
            SynchronizeClients(commandResultByPeers, synchronizer);

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < _millisecondsPerUpdate)
            {
                System.Threading.Thread.Sleep((int)(_millisecondsPerUpdate - stopwatch.ElapsedMilliseconds));
            }
            else
            {
                _logger.Log(LogLevel.Warn, $"Server update took too long: {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        public void Terminate(CancellationToken token)
        {

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

            // Add the server command results to all clients
            if (peerCommandResults.ContainsKey(-1))
            {
                foreach(var peerId in peerCommandResults.Keys)
                {
                    if (peerId == -1)
                    {
                        continue;
                    }

                    peerCommandResults[peerId] = peerCommandResults[peerId].Append(peerCommandResults[-1]);
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
                    return new GameCommandResult();
                }

                if (request.Input.Command != "join" && string.IsNullOrEmpty(player.Guid))
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {request.PeerId} joined no games.");
                    return new GameCommandResult();
                }
            }

            var synchronizer = new StateSynchronizer<GameState>(_serverState.GameState);
            var command = _commandFactory.Get(request.Input.Command);
            
            if (command == null)
            {
                _logger.Log(LogLevel.Info, $"Command not registered : {request.Input.Command}");
            }
            else if (command.ValidateParameters(request.Input))
            {
                return command.Execute(new GameCommandContext(_networkAdapter, _serverState.GameState, player));
            }

            return new GameCommandResult();
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
