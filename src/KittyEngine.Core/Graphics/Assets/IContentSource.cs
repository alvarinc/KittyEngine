using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public interface IContentSource
    {
        ContentSourceDescription Description { get; }
        string[] GetResources(AssetType contentType);
        Stream GetStream(AssetType contentType, string resourceName);
        BitmapImage Get(AssetType contentType, string resourceName);
        Bitmap GetBitmap(AssetType contentType, string resourceName);
    }
}
