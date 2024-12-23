namespace KittyEngine.Core.Client
{
    using KittyEngine.Core.Server;
    using KittyEngine.Core.State;
    using LiteNetLib;
    using LiteNetLib.Utils;
    using Newtonsoft.Json;
    using System;

    public class Client
    {
        private NetManager _client;
        private EventBasedNetListener _listener;
        private IClientGameLogic _gameLogic;
        private Player _player;
        private bool _inGame = false;

        public Client(IClientGameLogic gameLogic)
        {
            _gameLogic = gameLogic;

            ConfigureClient();
        }

        public void Run(GameServer gameServer, Player player)
        {
            _player = player;

            Console.WriteLine("[Client] Connecting...");

            _client.Start();
            _client.Connect(gameServer.Address, gameServer.Port, "Client=KittyEngine.Core.Client");

            Console.WriteLine("[Client] Press keys to send to server. Press ESC to stop.");
            GameLoop();

            _client.Stop();
        }

        private void ConfigureClient()
        {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);

            _listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine($"Connected to server: {peer}");
                _gameLogic.ViewAs(_player.Guid);

                Console.WriteLine($"Join game as {_player.Name}");
                var cmd = new GameCommandInput("join");
                cmd.Args["guid"] = _player.Guid;
                cmd.Args["name"] = _player.Name;
                SendMessage(cmd);
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

        private void GameLoop()
        {
            _inGame = true;
            while (_inGame)
            {
                var inputs = _gameLogic.HandleInputEvents();
                foreach (var input in inputs)
                {
                    SendMessage(input);
                }

                HandleServerEvents();

                _gameLogic.RenderOutput();

                Thread.Sleep(15);
            }
        }

        private void HandleServerEvents()
        {
            _client.PollEvents();
        }

        public void SendMessage(GameCommandInput input)
        {
            if (input != null && _client != null && _client.FirstPeer != null && _client.FirstPeer.ConnectionState == ConnectionState.Connected)
            {
                var writer = new NetDataWriter();
                var json = JsonConvert.SerializeObject(input);
                writer.Put(json);
                _client.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }
    }
}
