namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Model;
    using KittyEngine.Core.Server;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.Services.Network;
    using KittyEngine.Core.State;
    using LiteNetLib;
    using Newtonsoft.Json;
    using System;
    using System.Reflection.PortableExecutable;

    public class Client
    {
        private NetManager _client;
        private EventBasedNetListener _listener;
        private NetworkAdapter _networkAdapter;
        private LargeMessageReceiver _largeMessageReceiver;

        private ILogger _logger;
        private IClientGameLogic _gameLogic;
        private ClientState _clientState;

        private PlayerInput _player;
        private bool _inGame = false;

        public Client(ILogger logger, IClientGameLogic gameLogic, ClientState clientState)
        {
            _logger = logger;
            _gameLogic = gameLogic;
            _clientState = clientState;

            ConfigureClient();
        }

        public void Run(PlayerInput player, ServerInput server)
        {
            Run(player, server, CancellationToken.None);
        }

        public void Run(PlayerInput player, ServerInput server, CancellationToken token)
        {
            if (player == null)
            {
                return;
            }

            _player = player;

            _logger.Log(LogLevel.Info, "[Client] Connecting...");

            _client.Start();
            _client.Connect(server.Address, server.Port, "Client=KittyEngine.Core.Client");

            _logger.Log(LogLevel.Info, "[Client] Press keys to send to server. Press ESC to stop.");

            _inGame = true;
            while (_inGame && !token.IsCancellationRequested)
            {
                _gameLogic.RenderLoop();
            }

            _gameLogic.Terminate(token);

            _client.Stop();
        }

        private void OnCompleteMessageReceived(NetPeer peer, string checksum, byte[] completeMessage)
        {
            var message = System.Text.Encoding.UTF8.GetString(completeMessage);
            _logger.Log(LogLevel.Debug, $"[Client] Received: {message}");

            try
            {
                var input = JsonConvert.DeserializeObject<GameCommandInput>(message);
                _gameLogic.HandleServerMessage(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ConfigureClient()
        {
            _largeMessageReceiver = new LargeMessageReceiver();
            _largeMessageReceiver.OnCompleteMessageReceived += OnCompleteMessageReceived;

            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);
            _networkAdapter = new NetworkAdapter(_client);
            _gameLogic.Bind(_networkAdapter);

            _listener.PeerConnectedEvent += peer =>
            {
                _logger.Log(LogLevel.Info, $"Connected to server: {peer}");

                _logger.Log(LogLevel.Info, $"Join game as {_player.Name}");
                var cmd = new GameCommandInput("join")
                    .AddArgument("guid", _player.Guid)
                    .AddArgument("name", _player.Name);

                _networkAdapter.SendMessage(cmd);
            };

            _listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
            {
                try
                {
                    Console.WriteLine($"Received data on channel {channel} from {peer} using {deliveryMethod}");
                    _largeMessageReceiver.OnNetworkReceive(peer, reader, deliveryMethod); // Delegate to existing handling
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing data on channel {channel}: {ex.Message}");
                }
            };

            _listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
                _logger.Log(LogLevel.Info, "[Client] Disconnected from server.");
                _inGame = false;
            };
        }
    }
}
