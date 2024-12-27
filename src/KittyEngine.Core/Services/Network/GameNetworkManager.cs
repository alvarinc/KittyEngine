using LiteNetLib;
using System.Net;
using System.Net.Sockets;

namespace KittyEngine.Core.Services.Network
{
    public interface IGameNetworkManager
    {
        void Start(int port);
        void Connect(string address, int port);
        void PollEvents();
        void Stop();

        void SendLargeMessage(NetPeer peer, byte[] data);
    }

    public class GameNetworkManager : INetEventListener, IGameNetworkManager
    {
        private NetManager netManager;
        private LargeMessageSender sender;
        private LargeMessageReceiver receiver;

        public GameNetworkManager()
        {
            sender = new LargeMessageSender();
            receiver = new LargeMessageReceiver();
            receiver.OnCompleteMessageReceived += OnCompleteMessageReceived;
            netManager = new NetManager(this);
        }

        public void Start(int port)
        {
            if (!netManager.Start(port))
            {
                Console.WriteLine("Failed to start NetManager!");
                return;
            }

            Console.WriteLine($"Server started on port {port}");
        }

        public void Connect(string address, int port)
        {
            var peer = netManager.Connect(address, port, "SomeConnectionKey");
            if (peer != null)
            {
                Console.WriteLine($"Connecting to {address}:{port}");
            }
            else
            {
                Console.WriteLine($"Failed to connect to {address}:{port}");
            }
        }

        public void PollEvents()
        {
            netManager.PollEvents();
        }

        public void Stop()
        {
            netManager.Stop();
            Console.WriteLine("Server stopped.");
        }

        public void SendLargeMessage(NetPeer peer, byte[] data)
        {
            sender.SendLargeMessage(peer, data);
        }

        private void OnCompleteMessageReceived(NetPeer peer, string checksum, byte[] completeMessage)
        {
            // Handle the complete message here
            string messageContent = System.Text.Encoding.UTF8.GetString(completeMessage);
            Console.WriteLine($"Received complete message: {messageContent}");
        }

        // --- INetEventListener Implementation ---

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"Peer connected: {peer}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"Peer disconnected: {peer}, Reason: {disconnectInfo.Reason}");
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (request.Data.GetString() == "SomeConnectionKey")
            {
                request.Accept();
                Console.WriteLine($"Accepted connection request from {request.RemoteEndPoint}");
            }
            else
            {
                request.Reject();
                Console.WriteLine($"Rejected connection request from {request.RemoteEndPoint}");
            }
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            try
            {
                receiver.OnNetworkReceive(peer, reader, deliveryMethod);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing received data: {ex.Message}");
            }
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            try
            {
                Console.WriteLine($"Received data on channel {channelNumber} from {peer} using {deliveryMethod}");
                receiver.OnNetworkReceive(peer, reader, deliveryMethod); // Delegate to existing handling
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing data on channel {channelNumber}: {ex.Message}");
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Console.WriteLine($"Received unconnected message from {remoteEndPoint}, Type: {messageType}");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Console.WriteLine($"Network error occurred. Endpoint: {endPoint}, SocketErrorCode: {socketError}");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            Console.WriteLine($"Latency update from {peer}: {latency} ms");
        }
    }
}