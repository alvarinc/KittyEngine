using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    /// <summary>
    /// Describe a 3D model to use in a BVH Tree
    /// </summary>
    /// <typeparam name="TModel3DReference">Type referencing data linked to your 3D model</typeparam>
    public class BVHTreeNodeInfos<TModel3DReference>
    {
        public Rect3D Bounds { get; set; }
        public TModel3DReference Model3D { get; set; }
    }
}
