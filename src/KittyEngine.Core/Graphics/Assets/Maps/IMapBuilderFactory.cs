
namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public interface IMapBuilderFactory
    {
        void RegisterMap(IMapBuilder mapBuilder);
        string[] GetMaps();
        IMapBuilder Get(string map);
    }
}
