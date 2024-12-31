using KittyEngine.Core.Graphics;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    /// <summary>
    /// Describe a BVH Tree Node
    /// </summary>
    /// <typeparam name="TModel3DReference">Type referencing data linked to your 3D model</typeparam>
    public class BVHTreeNode<TModel3DReference>
    {
        public Rect3D Bounds { get; set; }
        public Point3D Center { get; set; }
        public TModel3DReference Model3D { get; set; }

        public BVHTreeNode<TModel3DReference> Left { get; set; }
        public BVHTreeNode<TModel3DReference> Right { get; set; }

        public BVHTreeNode()
        {
        }

        public BVHTreeNode(BVHTreeNodeInfos<TModel3DReference> infos)
        {
            Bounds = infos.Bounds;
            Model3D = infos.Model3D;
            Center = Rect3DHelper.GetCenter(infos.Bounds);
        }

        /// <summary>
        /// Returns intersections between rect parameter and data into the BVH Tree
        /// </summary>
        /// <param name="rect">Bound box used to test collisions</param>
        /// <returns>List of collided BVH leaf nodes</returns>
        public List<BVHTreeNode<TModel3DReference>> GetIntersected(Rect3D rect)
        {
            var list = new List<BVHTreeNode<TModel3DReference>>();
            if (rect.IntersectsWith(Bounds))
            {
                if (Model3D != null)
                {
                    list.Add(this);
                }
                else 
                {
                    if (Left != null)
                    {
                        list.AddRange(Left.GetIntersected(rect));
                    }

                    if (Right != null)
                    {
                        list.AddRange(Right.GetIntersected(rect));
                    }
                }
            }

            return list;
        }
    }
}
