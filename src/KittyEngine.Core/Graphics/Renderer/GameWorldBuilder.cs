using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.Graphics.Renderer
{
    public class GameWorldBuilder : IGameWorldBuilder
    {
        private ILayeredModel3DFactory _layeredModel3DFactory;

        public GameWorldBuilder(ILayeredModel3DFactory layeredModel3DFactory)
        {
            _layeredModel3DFactory = layeredModel3DFactory;
        }

        public List<LayeredModel3D> Create(MapDefinition mapDefinition)
        {
            var level = new List<LayeredModel3D>();

            var definitions = new List<ModelBaseDefinition>();
            definitions.AddRange(mapDefinition.Lights);
            definitions.AddRange(mapDefinition.Skyboxes);
            definitions.AddRange(mapDefinition.Volumes);

            definitions = definitions.OrderBy(x => x.Index).ToList();

            foreach (var definition in definitions)
            {
                level.Add(_layeredModel3DFactory.Build(definition));
            }

            return level;
        }
    }
}
