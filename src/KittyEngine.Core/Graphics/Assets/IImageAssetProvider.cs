using KittyEngine.Core.Graphics.Models.Builders;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public interface IImageAssetProvider
    {
        MaterialGroup CreateMaterial(string filename);
        MaterialGroup CreateMaterial(string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.UniformToFill);
        MaterialGroup CreateMaterial(string filename, TileMode tileMode = TileMode.Tile, Stretch stretch = Stretch.UniformToFill, double ratioX = 1, double ratioY = 1);
        System.Windows.Media.Brush CreateTextureBrush(string filename);
        BitmapImage Get(AssetType contentType, string resourceName);
        Bitmap GetBitmap(AssetType contentType, string resourceName);
        BitmapImage GetSkyboxPart(string resourceName, SkyboxFace face);
    }
}