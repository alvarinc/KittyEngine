
namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapBuilderFromFile : MapBuilderFromStream
    {
        public MapBuilderFromFile(string path)
            : base(System.IO.File.OpenRead(path))
        {
        }
    }
}
