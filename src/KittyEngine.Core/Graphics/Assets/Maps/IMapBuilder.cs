using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public interface IMapBuilder
    {
        string MapName { get; }

        MapDefinition CreateMap();

        Point3D PlayerPosition { get; }

        Vector3D PlayerLookDirection { get; }
    }
}
