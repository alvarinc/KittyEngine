using LiteNetLib.Utils;
using LiteNetLib;
using KittyEngine.Core.Common;

namespace KittyEngine.Core.Services.Network
{
    public class LargeMessageReceiver
    {
        // Delegate for ProcessCompleteMessage event
        public delegate void CompleteMessageReceivedHandler(NetPeer peer, string checksum, byte[] completeMessage);

        // Event that is raised when a complete message has been processed
        public event CompleteMessageReceivedHandler OnCompleteMessageReceived;

        private const int RetryInterval = 100; // Milliseconds
        private Dictionary<string, ReceivedMessage> receivedMessages = new Dictionary<string, ReceivedMessage>();

        // Handle incoming chunks and manage retry requests
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            string checksum = reader.GetString();
            ushort chunkIndex = reader.GetUShort();
            ushort totalChunks = reader.GetUShort();
            byte[] chunkData = reader.GetRemainingBytes();

            if (!receivedMessages.ContainsKey(checksum))
            {
                receivedMessages[checksum] = new ReceivedMessage(totalChunks);
            }

            var message = receivedMessages[checksum];
            message.AddChunk(chunkIndex, chunkData);

            if (message.IsComplete)
            {
                byte[] compressedData = message.ReassembleMessage();
                byte[] originalData = DataUtils.Decompress(compressedData);

                receivedMessages.Remove(checksum);
                OnProcessCompleteMessage(peer, checksum, originalData);
            }
            else if (message.ShouldRetry())
            {
                RequestMissingChunks(peer, checksum, message.GetMissingChunks());
                message.UpdateLastRetryTime();
            }

            reader.Recycle();
        }

        // Request missing chunks from the sender
        private void RequestMissingChunks(NetPeer peer, string checksum, IEnumerable<ushort> missingChunks)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(checksum);
            writer.Put((ushort)missingChunks.Count());
            foreach (var chunkIndex in missingChunks)
            {
                writer.Put(chunkIndex);
            }

            peer.Send(writer, DeliveryMethod.ReliableUnordered);
        }

        // Handle the complete message
        private void OnProcessCompleteMessage(NetPeer peer, string checksum, byte[] data)
        {
            OnCompleteMessageReceived?.Invoke(peer, checksum, data);
        }

        // Internal class to track received chunks
        private class ReceivedMessage
        {
            private readonly Dictionary<ushort, byte[]> chunks = new Dictionary<ushort, byte[]>();
            private readonly ushort totalChunks;
            private DateTime lastRetryTime;

            public ReceivedMessage(ushort totalChunks)
            {
                this.totalChunks = totalChunks;
                lastRetryTime = DateTime.Now;
            }

            public void AddChunk(ushort index, byte[] data)
            {
                if (!chunks.ContainsKey(index))
                {
                    chunks[index] = data;
                }
            }

            public bool IsComplete => chunks.Count == totalChunks;

            public byte[] ReassembleMessage()
            {
                return chunks.OrderBy(kvp => kvp.Key)
                             .SelectMany(kvp => kvp.Value)
                             .ToArray();
            }

            public IEnumerable<ushort> GetMissingChunks()
            {
                return Enumerable.Range(0, totalChunks)
                                 .Where(i => !chunks.ContainsKey((ushort)i))
                                 .Select(i => (ushort)i);
            }

            public bool ShouldRetry()
            {
                return (DateTime.Now - lastRetryTime).TotalMilliseconds > RetryInterval;
            }

            public void UpdateLastRetryTime()
            {
                lastRetryTime = DateTime.Now;
            }
        }
    }
}
