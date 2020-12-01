using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction.Octree
{
    interface IOctreeNode
    {
        IOctreeNode[] GetNodes();
        int AddColor(Color color, int index);
        Color GetColor(Color color, int index);
    }

    class OctreeNode : IOctreeNode
    {
        private IOctreeNode[] nodes;
        public OctreeNode()
        {
            nodes = new IOctreeNode[8];
        }
        private int ColorImportance(int color, int index)
        {
            return ((color & (255 >> index)) >= (128 >> index)) ? 1 : 0;
        }
        public int AddColor(Color color, int index)
        {
            int nodeId = (ColorImportance(color.R, index) +
                        2 * ColorImportance(color.G, index) +
                        4 * ColorImportance(color.B, index));
            if (nodes[nodeId] == null)
            {
                if(index == 6)
                {
                    nodes[nodeId] = new OctreeLeaf(color);
                }
                else
                {
                    nodes[nodeId] = new OctreeNode();
                    nodes[nodeId].AddColor(color, index + 1);
                }
                return 1;
            }
            return nodes[nodeId].AddColor(color, index + 1);
        }

        public Color GetColor(Color color, int index)
        {
            int nodeId = (ColorImportance(color.R, index) +
                        2 * ColorImportance(color.G, index) +
                        4 * ColorImportance(color.B, index));
            return nodes[nodeId].GetColor(color, index + 1);
        }

        public IOctreeNode[] GetNodes()
        {
            return nodes;
        }
    }

    class OctreeLeaf : IOctreeNode
    {
        private Color color;
        private int Count;

        public OctreeLeaf(Color color)
        {
            this.color = color;
            Count = 1;
        }
        public int AddColor(Color color, int index)
        {
            Count++;
            return 0;
        }

        public Color GetColor(Color color, int index)
        {
            return color;
        }

        public IOctreeNode[] GetNodes()
        {
            return null;
        }
    }
}
