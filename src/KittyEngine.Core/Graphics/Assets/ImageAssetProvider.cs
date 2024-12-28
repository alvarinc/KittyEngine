using KittyEngine.Core.Graphics;
using KittyEngine.Core.Graphics.Models.Builders;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public class ImageAssetProvider : IImageAssetProvider
    {
        private Dictionary<string, BitmapImage> _skyboxCache = new Dictionary<string, BitmapImage>();
        private IContentService _contentService;

        public ImageAssetProvider(IContentService contentService)
        {
            _contentService = contentService;
        }

        public BitmapImage Get(AssetType contentType, string resourceName)
        {
            return _contentService.Get(contentType, resourceName);
        }

        public Bitmap GetBitmap(AssetType contentType, string resourceName)
        {
            return _contentService.GetBitmap(contentType, resourceName);
        }

        /// <summary>
        /// Get part of skybox.
        /// </summary>
        /// <param name="shortResourceName">path to resource</param>
        /// <param name="column">column in skybox image</param>
        /// <param name="row">row in skybox image</param>
        /// <returns></returns>
        public BitmapImage GetSkyboxPart(string resourceName, SkyboxFace face)
        {
            BitmapImage bitmap = null;

            var key = $"{resourceName}|{face}";
            if (!_skyboxCache.TryGetValue(key, out bitmap))
            {
                bitmap = BuildSkyboxPart(resourceName, face);
                _skyboxCache.Add(key, bitmap);
            }

            return bitmap;
        }

        private BitmapImage BuildSkyboxPart(string resourceName, SkyboxFace face)
        {
            var imageSource = GetBitmap(AssetType.Skybox, resourceName);
            if (imageSource == null)
            {
                return CreateNotFoundImage($"Not found :'({System.Environment.NewLine}{resourceName}");
            }

            var sizeX = imageSource.Size.Width / 4;
            var sizeY = imageSource.Size.Height / 3;
            int x = 0;
            int y = 0;

            switch (face)
            {
                case SkyboxFace.Front:
                    {
                        x = sizeX * 2;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Back:
                    {
                        x = sizeX * 0;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Top:
                    {
                        x = sizeX * 2;
                        y = sizeY * 0;
                        break;
                    }
                case SkyboxFace.Bottom:
                    {
                        x = sizeX * 2;
                        y = sizeY * 2;
                        break;
                    }
                case SkyboxFace.Right:
                    {
                        x = sizeX * 3;
                        y = sizeY * 1;
                        break;
                    }
                case SkyboxFace.Left:
                    {
                        x = sizeX * 1;
                        y = sizeY * 1;
                        break;
                    }
            }

            Rectangle cloneRect = new Rectangle(x, y, sizeX, sizeY);
            Bitmap partialImage = imageSource.Clone(cloneRect, imageSource.PixelFormat);

            return partialImage.ToBitmapImage();
        }

        public MaterialGroup CreateMaterial(string filename)
        {
            return CreateMaterial(filename, TileMode.Tile, Stretch.UniformToFill, 1);
        }

        public MaterialGroup CreateMaterial(string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.UniformToFill)
        {
            return CreateMaterial(filename, tileMode, stretch, 1);
        }

        public MaterialGroup CreateMaterial(string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.UniformToFill, double ratioX = 1, double ratioY = 1)
        {
            var materiaGroup = new MaterialGroup();

            var imageSource = Get(AssetType.Texture, filename);

            if (imageSource == null)
            {
                imageSource = CreateNotFoundImage($"Not found :'({System.Environment.NewLine}{filename}");
            }

            var brush = new ImageBrush(imageSource);
            brush.TileMode = tileMode;
            brush.Stretch = stretch;
            brush.Viewport = new Rect(new System.Windows.Point(0, 0), new System.Windows.Point(ratioX, ratioY));
            brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            materiaGroup.Children.Add(new DiffuseMaterial(brush));

            return materiaGroup;
        }

        public System.Windows.Media.Brush CreateTextureBrush(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return new SolidColorBrush(Colors.Transparent);
            }

            return new ImageBrush(Get(AssetType.Texture, filename));
        }

        private BitmapImage CreateNotFoundImage(string message)
        {
            var xSize = 101;
            var ySize = 101;
            var bitmap = new Bitmap(xSize, ySize);
            using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                var font = new Font(System.Drawing.FontFamily.GenericSansSerif, 10, System.Drawing.FontStyle.Regular);
                graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), new Rectangle(0, 0, xSize, ySize));
                graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black, 2), new Rectangle(0, 0, xSize, ySize));
                graphics.DrawString(message, font, new SolidBrush(System.Drawing.Color.Red), new RectangleF { X = 1, Y = 1, Width = xSize - 1, Height = ySize - 1 }, sf);
            }

            return bitmap.ToBitmapImage();
        }
    }
}
