using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colors_reduction.OctreeTree
{
    class Octree
    {
        private IOctreeNode root;
        public int ColorsCount { get; private set; }

        public Octree()
        {
            root = new OctreeNode();
            ColorsCount = 0;
        }

        public void AddColor(Color color)
        {
            ColorsCount += root.AddColor(color, 0);
        }

        public Color GetColor(Color color)
        {
            return root.GetColor(color, 0);
        }

        public void ReduceColorsCount(int limit)
        {
            while (ColorsCount > limit)
            {
                int CountOfReducedColors = 0;
                IOctreeNode ReducedNode;
                IOctreeNode ReducedNodeParent;
                (ReducedNodeParent, ReducedNode) = GetReducedNode(root);
                foreach (IOctreeNode node in ReducedNode.GetNodes())
                {
                    if (node != null) CountOfReducedColors++;
                }
                if (ReducedNodeParent == null)
                {
                    root = new OctreeLeaf(ReducedNode.GetAvgColor(), ReducedNode.GetColorCount());
                }
                else for (int i = 0; i < 8; i++)
                {
                    if (ReducedNodeParent.GetNodes()[i] == ReducedNode)
                    {
                        ReducedNodeParent.GetNodes()[i] = new OctreeLeaf(ReducedNode.GetAvgColor(), ReducedNode.GetColorCount());
                    }
                }
                ColorsCount -= CountOfReducedColors;
                ColorsCount++;
            }
        }

        private (IOctreeNode parent, IOctreeNode node) GetReducedNode(IOctreeNode root)
        {
            int maxIndex = 0;
            int minColorCount = int.MaxValue;
            IOctreeNode parent = null;
            IOctreeNode node = null;
            DFS(root, null, null, ref node, ref parent, 0, ref maxIndex, ref minColorCount);
            return (parent, node);
        }

        private void DFS(IOctreeNode node, IOctreeNode parent, IOctreeNode parentParent,
                         ref IOctreeNode rNode, ref IOctreeNode rParentNode,
                         int index, ref int maxIndex, ref int minColorCount
                        )
        {
            if (node == null) return;
            if (node.GetNodes() == null)
            {
                if (index > maxIndex || (index == maxIndex && parent.GetColorCount() < minColorCount))
                {
                    rNode = parent;
                    rParentNode = parentParent;
                    maxIndex = index;
                    minColorCount = parent.GetColorCount();
                }
                return;
            }
            foreach (IOctreeNode n in node.GetNodes())
            {
                DFS(n, node, parent, ref rNode, ref rParentNode, index + 1, ref maxIndex, ref minColorCount);
            }
        }
    }
}


