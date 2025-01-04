namespace KittyEngine.Core.Server
{
    using KittyEngine.Core.Services.Configuration;
    using KittyEngine.Core.Services.Logging;
    using KittyEngine.Core.Services.Network;
    using LiteNetLib;
    using Newtonsoft.Json;
    using System;

    public class Server
    {
        private NetManager _server;
        private EventBasedNetListener _listener;
        private LargeMessageReceiver _largeMessageReceiver;

        private ILogger _logger;
        private IConfigurationService _configuration;
        private IServerGameLogic _gameLogic;

        public Server(ILogger logger, IConfigurationService configuration, IServerGameLogic gameLogic)
        {
            _logger = logger;
            _configuration = configuration;
            _gameLogic = gameLogic;

            ConfigureServer();
        }

        public void Run(int port = 0)
        {
            Run(port, CancellationToken.None);
        }

        public void Run(int port, CancellationToken token)
        {
            if (port == 0)
            {
                port = _configuration.GetDefaultServer().Port;
            }

            _server.Start(port);

            while (!token.IsCancellationRequested)
            {
                _gameLogic.GameLoop();
            }

            _gameLogic.Terminate(token);

            _server.Stop();
        }

        public void SendMessage(GameCommandInput input)
        {
            _gameLogic.OnMessageReceived(null, input);
        }

        private void OnCompleteMessageReceived(NetPeer peer, string checksum, byte[] completeMessage)
        {
            var message = System.Text.Encoding.UTF8.GetString(completeMessage);
            _logger.Log(LogLevel.Info, $"[Server] Player {peer.Id} : command {message}");
            GameCommandInput input = null;

            try
            {
                input = JsonConvert.DeserializeObject<GameCommandInput>(message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Info, $"[Server] invalid message from {peer.Id}");
            }

            if (input != null)
            {
                _gameLogic.OnMessageReceived(peer, input);
            }
        }

        private void ConfigureServer()
        {
            _largeMessageReceiver = new LargeMessageReceiver();
            _largeMessageReceiver.OnCompleteMessageReceived += OnCompleteMessageReceived;

            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);
            _gameLogic.Bind(new NetworkAdapter(_server));

            _listener.ConnectionRequestEvent += request =>
            {
                if (_server.ConnectedPeersCount < 10 && request.Data.GetString() == "Client=KittyEngine.Core.Client")
                    request.Accept();
                else
                    request.Reject();
            };

            _listener.PeerConnectedEvent += peer =>
            {
                _gameLogic.OnClientConnected(peer);
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
        }
    }
}
