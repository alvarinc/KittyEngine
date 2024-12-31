using System.Collections.Generic;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    /// <summary>
    /// Create leaf nodes for BVH Tree
    /// </summary>
    /// <typeparam name="TModel3DReference">Type referencing data linked to your 3D model</typeparam>
    public interface IBVHLeafNodeFactory<TModel3DReference>
    {
        IEnumerable<BVHTreeNodeInfos<TModel3DReference>> CreateLeafNodes();
    }
}
