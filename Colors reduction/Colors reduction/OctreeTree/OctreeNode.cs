using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction.OctreeTree
{
    interface IOctreeNode
    {
        IOctreeNode[] GetNodes();
        int AddColor(Color color, int index);
        Color GetColor(Color color, int index);
        int GetColorCount();
        Color GetAvgColor();
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
                if(index == 7)
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

        public int GetColorCount()
        {
            int colorCount = 0;
            for(int i=0;i<8;i++)
            {
                if(nodes[i]!=null)
                {
                    colorCount += nodes[i].GetColorCount();
                }
            }
            return colorCount;
        }

        public Color GetAvgColor()
        {
            int anvColorR = 0;
            int anvColorG = 0;
            int anvColorB = 0;
            int n = 0;
            foreach (IOctreeNode node in nodes)
            {
                if (node != null)
                {
                    Color avgColor = node.GetAvgColor();
                    anvColorR += avgColor.R;
                    anvColorG += avgColor.G;
                    anvColorB += avgColor.B;
                    n += node.GetColorCount();
                }
            }
            if(n > 0)
            {
                return Color.FromArgb(anvColorR / n, anvColorG / n,anvColorB / n);
            }
            else
            {
                throw new Exception("Bad tree");
            }
        }
    }

    class OctreeLeaf : IOctreeNode
    {
        private Color color;
        private int Count;

        public OctreeLeaf(Color color, int count = 1)
        {
            this.color = color;
            Count = 1;
        }
        public int AddColor(Color color, int index)
        {
            Count++;
            return 0;
        }

        public Color GetAvgColor()
        {
            return color;
        }

        public Color GetColor(Color color, int index)
        {
            return color;
        }

        public int GetColorCount()
        {
            return Count;
        }

        public IOctreeNode[] GetNodes()
        {
            return null;
        }
    }
}
