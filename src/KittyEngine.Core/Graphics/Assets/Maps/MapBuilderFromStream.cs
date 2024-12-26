using Newtonsoft.Json;
using KittyEngine.Core.Graphics.Models.Definitions;
using System.IO;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapBuilderFromStream : IMapBuilder
    {
        public string MapName => _mapBuilder.MapName;

        public Point3D PlayerPosition => _mapBuilder.PlayerPosition;

        public Vector3D PlayerLookDirection => _mapBuilder.PlayerLookDirection;

        private MapBuilderFromDefinition _mapBuilder;

        public MapBuilderFromStream(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var mapDefinition = JsonConvert.DeserializeObject<MapDefinition>(json);
                _mapBuilder = new MapBuilderFromDefinition(mapDefinition);
            }
        }

        public MapDefinition CreateMap()
        {
            return _mapBuilder.CreateMap();
        }
    }
}
