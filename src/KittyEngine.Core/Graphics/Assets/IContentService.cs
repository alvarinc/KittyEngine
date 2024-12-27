using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public interface IContentService
    {
        void RegisterSource(IContentSource source);
        ContentSourceDescription[] GetSources();
        void RemoveSource(ContentSourceDescription sourceDescription);

        string[] GetResources(AssetType contentType);
        Stream GetStream(AssetType contentType, string resourceName);
        BitmapImage Get(AssetType contentType, string resourceName);
        Bitmap GetBitmap(AssetType contentType, string resourceName);
    }
}
