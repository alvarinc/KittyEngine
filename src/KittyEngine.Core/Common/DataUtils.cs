using System.IO.Compression;
using System.IO;
using System.Security.Cryptography;

namespace KittyEngine.Core.Common
{
    public static class DataUtils
    {
        // Compress data using GZip
        public static byte[] Compress(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        // Decompress data using GZip
        public static byte[] Decompress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var gzip = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }

        // Generate a checksum using SHA256
        public static string GenerateChecksum(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(data)).Replace("-", "");
            }
        }
    }
}
