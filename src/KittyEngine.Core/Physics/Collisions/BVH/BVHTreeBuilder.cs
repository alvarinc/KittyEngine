using KittyEngine.Core.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics.Collisions.BVH
{
    /// <summary>
    /// BVH Tree builder
    /// </summary>
    /// <typeparam name="TModel3DReference"></typeparam>
    public class BVHTreeBuilder<TModel3DReference>
    {
        /// <summary>
        /// Build a BVH Tree based on data provided by leadNodeFactory parameter
        /// </summary>
        /// <param name="leadNodeFactory">Provide leaf node to use</param>
        /// <returns>Root node of the BVH Tree</returns>
        public BVHTreeNode<TModel3DReference> Build(IBVHLeafNodeFactory<TModel3DReference> leadNodeFactory)
        {
            var leafNodes = leadNodeFactory.CreateLeafNodes().Select(x => new BVHTreeNode<TModel3DReference>(x));

            var root = new BVHTreeNode<TModel3DReference>
            {
                Bounds = Rect3DHelper.CreateBoundingBox(leafNodes.Select(x => x.Bounds))
            };

            CreateHierarchy(root, leafNodes);

            return root;
        }

        private void CreateHierarchy(BVHTreeNode<TModel3DReference> node, IEnumerable<BVHTreeNode<TModel3DReference>> leafNodes)
        {
            if (leafNodes.Count() == 1)
            {
                var leaf = leafNodes.First();
                node.Model3D = leaf.Model3D;
                node.Bounds = leaf.Bounds;
                node.Center = leaf.Center;
            }
            else if (leafNodes.Count() == 2)
            {
                node.Left = leafNodes.First();
                node.Right = leafNodes.Last();
            }
            else
            {
                var partition = CreatePartition(node.Bounds, leafNodes);

                node.Left = new BVHTreeNode<TModel3DReference>
                {
                    Bounds = Rect3DHelper.CreateBoundingBox(partition.LeftNodes.Select(x => x.Bounds)),
                };

                node.Right = new BVHTreeNode<TModel3DReference>
                {
                    Bounds = Rect3DHelper.CreateBoundingBox(partition.RightNodes.Select(x => x.Bounds)),
                };
                
                CreateHierarchy(node.Left, partition.LeftNodes);
                CreateHierarchy(node.Right, partition.RightNodes);
            }
        }

        private BVHPartition<TModel3DReference> CreatePartition(Rect3D bounds, IEnumerable<BVHTreeNode<TModel3DReference>> leafNodes)
        {
            // Sort by the most longer axis
            var sorter = new MergeSort();
            var sortedLeafNodes = leafNodes.ToList();
            
            if (bounds.SizeX > bounds.SizeY && bounds.SizeX > bounds.SizeZ)
            {
                sorter.Sort(sortedLeafNodes, x => x.Bounds.X);
            }
            else if (bounds.SizeY > bounds.SizeX && bounds.SizeY > bounds.SizeZ)
            {
                sorter.Sort(sortedLeafNodes, x => x.Bounds.Y);
            }
            else 
            {
                sorter.Sort(sortedLeafNodes, x => x.Bounds.Z);
            }

            // Partitionning
            int middle = sortedLeafNodes.Count / 2;
            var left = sortedLeafNodes.GetRange(0, middle + 1);
            var right  = sortedLeafNodes.GetRange(middle + 1, sortedLeafNodes.Count - 1 - middle);

            return new BVHPartition<TModel3DReference>
            {
                LeftNodes = left,
                RightNodes = right,
            };
        }

        class BVHPartition<TModel3DReference>
        {
            public IEnumerable<BVHTreeNode<TModel3DReference>> LeftNodes { get; set; }
            public IEnumerable<BVHTreeNode<TModel3DReference>> RightNodes { get; set; }
        }
    }
}
