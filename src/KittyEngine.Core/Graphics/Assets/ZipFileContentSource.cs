using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO.Compression;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public class ZipFileContentSource : IContentSource
    {
        private Dictionary<string, BitmapImage> _bitmapImageCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, Bitmap> _bitmapCache = new Dictionary<string, Bitmap>();
        private string _zipFilename;

        public ZipFileContentSource(string filename)
        {
            _zipFilename = filename;
        }

        public ContentSourceDescription Description
            => new ContentSourceDescription { Type = "File", Path = _zipFilename };

        public string[] GetResources(AssetType contentType)
        {
            if (File.Exists(_zipFilename))
            {
                var filenames = new List<string>();
                string folder = GetResourceDirectory(contentType);
                using (var zip = ZipFile.Open(_zipFilename, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        if (entry.FullName.StartsWith($"{folder}/"))
                        {
                            filenames.Add(entry.FullName.Substring(folder.Length + 1));
                        }
                    }
                }

                return filenames
                    .Where(x => x.EndsWith(".png") || x.EndsWith(".jpg"))
                    .ToArray();
            }
                
            return new string[] { };
        }

        public Stream GetStream(AssetType contentType, string resourceName)
        {
            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            
            using (var archive = ZipFile.OpenRead(_zipFilename))
            {
                var entry = archive.GetEntry(fullResourceName);
                if (entry != null)
                {
                    using (var zipStream = entry.Open())
                    {
                        var memoryStream = new MemoryStream();
                        zipStream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }

                return null;
            }
        }

        public BitmapImage Get(AssetType contentType, string resourceName)
        {
            if (!File.Exists(_zipFilename))
            {
                return null;
            }

            BitmapImage bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            if (!_bitmapImageCache.TryGetValue(fullResourceName, out bitmap))
            {
                using (var archive = ZipFile.OpenRead(_zipFilename))
                {
                    var entry = archive.GetEntry(fullResourceName);
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                zipStream.CopyTo(memoryStream);
                                memoryStream.Position = 0;

                                bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = memoryStream;
                                bitmap.EndInit();

                                _bitmapImageCache.Add(fullResourceName, bitmap);
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        public Bitmap GetBitmap(AssetType contentType, string resourceName)
        {
            if (!File.Exists(_zipFilename))
            {
                return null;
            }

            Bitmap bitmap = null;

            string folder = GetResourceDirectory(contentType);
            var fullResourceName = $"{folder}/{resourceName}";
            if (!_bitmapCache.TryGetValue(fullResourceName, out bitmap))
            {
                using (var archive = ZipFile.OpenRead(_zipFilename))
                {
                    var entry = archive.GetEntry(fullResourceName);
                    if (entry != null)
                    {
                        using (var zipStream = entry.Open())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                zipStream.CopyTo(memoryStream);
                                memoryStream.Position = 0;

                                bitmap = new Bitmap(memoryStream);
                                _bitmapCache.Add(fullResourceName, bitmap);
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        private string GetResourceDirectory(AssetType contentType)
        {
            switch (contentType)
            {
                case AssetType.Texture:
                    return "textures";
                case AssetType.Skybox:
                    return "skyboxes";
                case AssetType.Model:
                    return "models";
                case AssetType.Map:
                    return "maps";
                default:
                    return "misc";
            }
        }
    }
}
