namespace KittyEngine.Core.Server
{
    using KittyEngine.Core.Server.Commands;
    using KittyEngine.Core.State;
    using LiteNetLib;
    using System;
    using System.Collections.Generic;

    public interface IServerGameLogic
    {
        void Bind(NetworkAdapter networkAdapter);

        void OnClientConnected(NetPeer peer);

        void OnMessageReceived(NetPeer peer, GameCommandInput input);

        void GameLoop();
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

        private NetworkAdapter _networkAdapter;
        private readonly Dictionary<int, Player> _connectedUsers = new();
        private readonly Dictionary<int, GameCommandResult> _peerCommandResults = new();
        private readonly GameState _gameState = new();
        private readonly ICommandFactory _commandFactory;
        private readonly Queue<GameCommandRequest> _gameCommmandRequests = new();

        public ServerGameLogic(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
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
            _connectedUsers.Add(peer.Id, new Player());
            Console.WriteLine($"[Server] Player connected. ID: {peer.Id}");
        }

        public void OnMessageReceived(NetPeer peer, GameCommandInput input)
        {
            EnsureIsConnected();

            _gameCommmandRequests.Enqueue(new GameCommandRequest(peer.Id, input));

        }

        public void GameLoop()
        {
            EnsureIsConnected();

            // Get clients inputs
            _networkAdapter.HandlePeersEvents();

            var synchronizer = new StateSynchronizer<GameState>(_gameState);

            // Update players
            ExecuteCommands();

            // Send updated state to clients
            SynchronizeClients(synchronizer);

            Thread.Sleep(15);
        }

        private void EnsureIsConnected()
        {
            if (_networkAdapter == null)
            {
                throw new InvalidOperationException("No network adapter connected.");
            }
        }

        private void ExecuteCommands()
        {
            _peerCommandResults.Clear();

            while (_gameCommmandRequests.Count > 0)
            {
                var request = _gameCommmandRequests.Dequeue();
                var result = ExecuteCommand(request);

                if (!_peerCommandResults.TryGetValue(request.PeerId, out GameCommandResult currentResult))
                {
                    _peerCommandResults[request.PeerId] = result;
                }
                else
                {
                    _peerCommandResults[request.PeerId] = currentResult.Append(result);
                }
            }
        }

        private GameCommandResult ExecuteCommand(GameCommandRequest request)
        {
            if (!_connectedUsers.TryGetValue(request.PeerId, out Player player))
            {
                Console.WriteLine($"[Server] Player {request.PeerId} : Received a message from an unknown player.");
                return new GameCommandResult();
            }

            if (request.Input.Command != "join" && string.IsNullOrEmpty(player.Guid))
            {
                Console.WriteLine($"[Server] Player {request.PeerId} joined no games.");
                return new GameCommandResult();
            }

            var synchronizer = new StateSynchronizer<GameState>(_gameState);
            var command = _commandFactory.Create(request.Input.Command);

            if (command == null)
            {
                Console.WriteLine($"Command not registered : {request.Input.Command}");
            }
            else if (command.ValidateParameters(request.Input))
            {
                return command.Execute(_gameState, player, request.PeerId);
            }

            return new GameCommandResult();
        }

        private void SynchronizeClients(StateSynchronizer<GameState> synchronizer)
        {
            if (!_peerCommandResults.Values.Any(x => x.StateUpdated))
            {
                return;
            }

            var patchCmd = new GameCommandInput("sync");
            patchCmd.Args["entity"] = "gamestate";
            patchCmd.Args["mode"] = "patch";
            patchCmd.Args["value"] = synchronizer.GetJsonPatch();

            var fullCmd = new GameCommandInput("sync");
            fullCmd.Args["entity"] = "gamestate";
            fullCmd.Args["mode"] = "full";
            fullCmd.Args["value"] = synchronizer.GetJson();

            foreach (var connectedPeer in _networkAdapter.GetConnectedPeers())
            {
                var mode = PeerSynchronizationMode.Patch;

                if (_peerCommandResults.TryGetValue(connectedPeer.Id, out GameCommandResult commandResult))
                {
                    if (commandResult.PeerInitializated)
                    {
                        mode = PeerSynchronizationMode.Full;
                    }
                }

                Console.WriteLine($"[Server] Player {connectedPeer.Id} : {mode} synchronize");
                _networkAdapter.SendMessage(connectedPeer, mode == PeerSynchronizationMode.Full ? fullCmd : patchCmd);

                if (!_gameState.Players.ContainsKey(connectedPeer.Id))
                {
                    connectedPeer.Disconnect();
                    _connectedUsers.Remove(connectedPeer.Id);
                }
            }
        }
    }
}
