using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.GameEngine.Graphics.Assets;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapBuilderFromAsset : MapBuilderFromStream
    {
        public MapBuilderFromAsset(IContentService contentService, string path)
            :base(contentService.GetStream(AssetType.Map, path))
        {
        }
    }
}
