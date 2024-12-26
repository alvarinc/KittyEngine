
namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public abstract class MapBuilderFromResource : MapBuilderFromStream
    {
        public MapBuilderFromResource(System.Reflection.Assembly resourceAssembly, string resourceName)
            : base(resourceAssembly.GetManifestResourceStream(resourceName))
        {
        }
    }
}
