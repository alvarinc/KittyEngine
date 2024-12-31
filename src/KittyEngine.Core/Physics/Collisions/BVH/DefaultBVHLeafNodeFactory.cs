using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics.Models.Definitions;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    /// <summary>
    /// Specific BVH Leaf Node Factory for Kitty Engine object type LayeredModel3D
    /// </summary>
    public class DefaultBVHLeafNodeFactory : IBVHLeafNodeFactory<LayeredModel3D>
    {
        private ILayeredModel3DFactory _layeredModel3DFactory;
        private IEnumerable<ModelBaseDefinition> _objectDefinitions;
        public DefaultBVHLeafNodeFactory(ILayeredModel3DFactory layeredModel3DFactory, IEnumerable<ModelBaseDefinition> objectDefinitions)
        {
            _layeredModel3DFactory = layeredModel3DFactory;
            _objectDefinitions = objectDefinitions;
        }

        public IEnumerable<BVHTreeNodeInfos<LayeredModel3D>> CreateLeafNodes()
        {
            foreach (var definition in _objectDefinitions)
            {
                var layeredModel = _layeredModel3DFactory.Build(definition, false);
                var bounds = layeredModel.GetModel().Content.Bounds;
                yield return new BVHTreeNodeInfos<LayeredModel3D>
                {
                    Model3D = layeredModel,
                    Bounds = bounds
                };
            }
        }
    }
}
