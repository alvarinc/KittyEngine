
using System.Linq;
using System.Collections.Generic;
using KittyEngine.Core.Graphics.Assets.Maps.Predefined;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapBuilderFactory : IMapBuilderFactory
    {
        private Dictionary<string, IMapBuilder> _mapBuilders;

        public MapBuilderFactory()
        {
            _mapBuilders = new Dictionary<string, IMapBuilder>();
        }

        public void RegisterMap(IMapBuilder mapBuilder)
        {
            _mapBuilders.Add(mapBuilder.MapName, mapBuilder);
        }

        public string[] GetMaps()
        {
            var allMaps = _mapBuilders.Keys.ToList();
            allMaps.Sort();
            return allMaps.ToArray();
        }

        public IMapBuilder Get(string map)
        {
            // 1 : Get from file
            if (map.StartsWith("file:"))
            {
                var filepath = map.Substring(5);
                return new MapBuilderFromFile(filepath);
            }

            // 2 : Get from predefined
            if (map == PredefinedMapNames.New)
            {
                return new NewMapBuilder();
            }

            // 3 : Get from registered maps
            if (_mapBuilders.ContainsKey(map))
            {
                return _mapBuilders[map];
            }

            // 4 : Return empty map
            return new EmptyMapBuilder();
        }
    }
}
