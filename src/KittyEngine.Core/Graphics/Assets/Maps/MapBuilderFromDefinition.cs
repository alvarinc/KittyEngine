using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapBuilderFromDefinition : IMapBuilder
    {
        public string MapName => _mapDefinition.Name;

        public Point3D PlayerPosition => _mapDefinition.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapDefinition.PlayerLookDirection;

        private MapDefinition _mapDefinition;

        public MapBuilderFromDefinition(MapDefinition mapDefinition)
        {
            _mapDefinition = mapDefinition;
        }

        public MapDefinition CreateMap()
        {
            return _mapDefinition;
        }
    }
}
