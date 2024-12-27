using LiteNetLib.Utils;
using LiteNetLib;
using KittyEngine.Core.Common;

namespace KittyEngine.Core.Services.Network
{
    public class LargeMessageSender
    {
        private const int MaxChunkSize = 1024;
        private Dictionary<string, Dictionary<ushort, byte[]>> sentMessages = new Dictionary<string, Dictionary<ushort, byte[]>>();

        // Compress and send a large message in chunks
        public void SendLargeMessage(NetPeer peer, byte[] data)
        {
            byte[] compressedData = DataUtils.Compress(data);
            ushort totalChunks = (ushort)((compressedData.Length + MaxChunkSize - 1) / MaxChunkSize);
            string checksum = DataUtils.GenerateChecksum(compressedData);

            var chunks = new Dictionary<ushort, byte[]>();
            for (ushort i = 0; i < totalChunks; i++)
            {
                int offset = i * MaxChunkSize;
                int chunkSize = Math.Min(MaxChunkSize, compressedData.Length - offset);

                byte[] chunk = new byte[chunkSize];
                Array.Copy(compressedData, offset, chunk, 0, chunkSize);
                chunks[i] = chunk;

                SendChunk(peer, checksum, i, totalChunks, chunk);
            }

            sentMessages[checksum] = chunks;
        }

        // Handle retry requests for missing chunks
        public void HandleRetryRequest(NetPeer peer, NetPacketReader reader)
        {
            string checksum = reader.GetString();
            ushort chunkCount = reader.GetUShort();
            List<ushort> missingChunks = new List<ushort>();

            for (int i = 0; i < chunkCount; i++)
            {
                missingChunks.Add(reader.GetUShort());
            }

            if (sentMessages.TryGetValue(checksum, out var chunks))
            {
                foreach (var chunkIndex in missingChunks)
                {
                    if (chunks.TryGetValue(chunkIndex, out var chunkData))
                    {
                        SendChunk(peer, checksum, chunkIndex, (ushort)chunks.Count, chunkData);
                    }
                }
            }
        }

        // Send a single chunk of data
        private void SendChunk(NetPeer peer, string checksum, ushort index, ushort totalChunks, byte[] chunk)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(checksum);
            writer.Put(index);
            writer.Put(totalChunks);
            writer.Put(chunk);

            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }
}
