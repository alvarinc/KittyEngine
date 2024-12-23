namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Client.Model;
    using KittyEngine.Core.Server;
    using LiteNetLib;
    using Newtonsoft.Json;
    using System;

    public class Client
    {
        private NetManager _client;
        private EventBasedNetListener _listener;
        private NetworkAdapter _networkAdapter;
        private IClientGameLogic _gameLogic;
        private PlayerInput _player;
        private bool _inGame = false;

        public Client(IClientGameLogic gameLogic)
        {
            _gameLogic = gameLogic;

            ConfigureClient();
        }

        public void Run(ServerInput server, PlayerInput player)
        {
            _player = player;

            Console.WriteLine("[Client] Connecting...");

            _client.Start();
            _client.Connect(server.Address, server.Port, "Client=KittyEngine.Core.Client");

            Console.WriteLine("[Client] Press keys to send to server. Press ESC to stop.");

            _inGame = true;
            while (_inGame)
            {
                _gameLogic.RenderLoop();
            }

            _client.Stop();
        }

        private void ConfigureClient()
        {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);
            _networkAdapter = new NetworkAdapter(_client);
            _gameLogic.Bind(_networkAdapter);

            _listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine($"Connected to server: {peer}");
                _gameLogic.ViewAs(_player.Guid);

                Console.WriteLine($"Join game as {_player.Name}");
                var cmd = new GameCommandInput("join");
                cmd.Args["guid"] = _player.Guid;
                cmd.Args["name"] = _player.Name;
                _networkAdapter.SendMessage(cmd);
            };

            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
            {
                string message = dataReader.GetString();
                Console.WriteLine($"[Client] Received: {message}");
                try
                {
                    var input = JsonConvert.DeserializeObject<GameCommandInput>(message);
                    _gameLogic.HandleServerMessage(input);
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex.ToString()); 
                }

                dataReader.Recycle();
            };

            _listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
                Console.WriteLine("[Client] Disconnected from server.");
                _inGame = false;
            };
        }
    }
}
