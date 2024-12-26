using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.Graphics.Assets.Maps
{
    public class MapRenderer : IMapRenderer
    {
        private MapDefinition _mapDefinition;

        public MapDefinition Definition => _mapDefinition;

        public MapRenderer(MapDefinition mapDefinition)
        {
            _mapDefinition = mapDefinition;
        }

        public List<LayeredModel3D> Render()
        {
            var level = new List<LayeredModel3D>();

            var definitions = new List<ModelBaseDefinition>();
            definitions.AddRange(_mapDefinition.Lights);
            definitions.AddRange(_mapDefinition.Skyboxes);
            definitions.AddRange(_mapDefinition.Volumes);

            definitions = definitions.OrderBy(x => x.Index).ToList();

            var builder = new ModelBuilderFromDefinition();
            foreach (var definition in definitions)
            {
                level.Add(builder.Build(definition));
            }

            return level;
        }
    }
}
