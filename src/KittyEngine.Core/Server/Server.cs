﻿namespace KittyEngine.Core.Server
{
    using LiteNetLib;
    using Newtonsoft.Json;
    using System;

    public class Server
    {
        private NetManager _server;
        private EventBasedNetListener _listener;
        private IServerGameLogic _gameLogic;

        public Server(IServerGameLogic gameLogic)
        {
            _gameLogic = gameLogic;

            ConfigureServer();
        }

        public void Run(int port)
        {
            Run(port, CancellationToken.None);
        }

        public void Run(int port, CancellationToken token)
        {
            _server.Start(port);

            while (!token.IsCancellationRequested)
            {
                _gameLogic.GameLoop();
            }

            _server.Stop();
        }

        private void ConfigureServer()
        {
            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);
            _gameLogic.Bind(new NetworkAdapter(_server));

            _listener.ConnectionRequestEvent += request =>
            {
                if (_server.ConnectedPeersCount < 10)
                    request.AcceptIfKey("Client=KittyEngine.Core.Client");
                else
                    request.Reject();
            };

            _listener.PeerConnectedEvent += peer =>
            {
                _gameLogic.OnClientConnected(peer);
            };

            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
            {
                string message = dataReader.GetString();
                Console.WriteLine($"[Server] Player {fromPeer.Id} : command {message}");
                GameCommandInput input = null;

                try
                {
                    input = JsonConvert.DeserializeObject<GameCommandInput>(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Server] invalid message from {fromPeer.Id}");
                }

                if (input != null)
                {
                    _gameLogic.OnMessageReceived(fromPeer, input);
                }

                dataReader.Recycle();
            };
        }
    }
}
