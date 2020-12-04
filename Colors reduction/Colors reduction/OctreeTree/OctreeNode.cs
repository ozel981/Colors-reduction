using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction.OctreeTree
{
    public interface IOctreeNode
    {
        IOctreeNode BestColorFamilyToReduce { get; }
        int Index { get; }

        (int newColorsCount, IOctreeNode bestColorFamilyToReduce) AddColor(Color color, int index);
        void ReduceColorsCount(IOctreeNode nodeToReduce, Color avgFamilyColor);
        Color GetColor(Color color);
        int GetColorPixelsCount();
        int GetColorsCount();
        Color GetAvgColor();
    }

    public class OctreeNode : IOctreeNode
    {
        private Dictionary<int, IOctreeNode> nodes;

        public IOctreeNode BestColorFamilyToReduce { get; private set; }
        public int Index { get; private set; }

        public OctreeNode(int index)
        {
            Index = index;
            nodes = new Dictionary<int, IOctreeNode>();
        }

        public (int newColorsCount, IOctreeNode bestColorFamilyToReduce) AddColor(Color color, int index)
        {
            int nodeKey = GetNodeKeyForColor(color);
            int colorsCount = 1;
            if (nodes.ContainsKey(nodeKey))
            {
                (int colorsCount, IOctreeNode newFamily) result = nodes[nodeKey].AddColor(color, index + 1);
                SetBestColorFamilyToReduce(result.newFamily);
                colorsCount = result.colorsCount;
            }
            else
            {
                if (index == 7)
                {
                    nodes.Add(nodeKey, new OctreeLeaf(color));
                    SetBestColorFamilyToReduce(this);
                }
                else
                {
                    IOctreeNode nextNode = new OctreeNode(index + 1);
                    nodes.Add(nodeKey, nextNode);
                    (int colorsCount, IOctreeNode newFamily) result = nextNode.AddColor(color, index + 1);
                    SetBestColorFamilyToReduce(result.newFamily);
                }
            }
            return (colorsCount, BestColorFamilyToReduce);
        }
        public Color GetColor(Color color)
        {
            int nodeKey = GetNodeKeyForColor(color);
            return nodes[nodeKey].GetColor(color);
        }
        public int GetColorPixelsCount()
        {
            int colorCount = 0;
            foreach (IOctreeNode node in nodes.Values)
            {
                colorCount += node.GetColorPixelsCount();
            }
            return colorCount;
        }
        public Color GetAvgColor()
        {
            int anvColorR = 0;
            int anvColorG = 0;
            int anvColorB = 0;
            int n = 0;
            foreach (IOctreeNode node in nodes.Values)
            {
                if (node != null)
                {
                    Color avgColor = node.GetAvgColor();
                    int colorCount = node.GetColorPixelsCount();
                    anvColorR += colorCount * avgColor.R;
                    anvColorG += colorCount * avgColor.G;
                    anvColorB += colorCount * avgColor.B;
                    n += colorCount;
                }
            }
            if (n > 0)
            {
                return Color.FromArgb(anvColorR / n, anvColorG / n, anvColorB / n);
            }
            else
            {
                throw new Exception("Bad tree");
            }
        }
        public void ReduceColorsCount(IOctreeNode nodeToReduce, Color avgFamilyColor)
        {
            if (nodes.Values.Contains(nodeToReduce))
            {
                int key = 0;
                foreach (var node in nodes)
                {
                    if (node.Value == nodeToReduce)
                    {
                        key = node.Key;
                        break;
                    }
                }
                nodes[key] = new OctreeLeaf(nodeToReduce.GetAvgColor(), nodeToReduce.GetColorPixelsCount(), Index + 1);
                BestColorFamilyToReduce = null;
                foreach (IOctreeNode octreeNode in nodes.Values)
                {
                    SetBestColorFamilyToReduce(octreeNode.BestColorFamilyToReduce);
                }
                SetBestColorFamilyToReduce(this);
            }
            else
            {
                int nodeKey = GetNodeKeyForColor(avgFamilyColor);
                nodes[nodeKey].ReduceColorsCount(nodeToReduce, avgFamilyColor);
                BestColorFamilyToReduce = null;
                foreach (IOctreeNode octreeNode in nodes.Values)
                {
                    SetBestColorFamilyToReduce(octreeNode.BestColorFamilyToReduce);
                }
            }
        }
        public int GetColorsCount()
        {
            int colorsCount = 0;
            foreach (IOctreeNode node in nodes.Values)
            {
                colorsCount += node.GetColorsCount();
            }
            return colorsCount;
        }
        private int GetNodeKeyForColor(Color color)
        {
            return (GetColorBit(color.R) +
                        2 * GetColorBit(color.G) +
                        4 * GetColorBit(color.B));
        }
        private int GetColorBit(int color)
        {
            return ((color & (255 >> Index)) >= (128 >> Index)) ? 1 : 0;
        }      
        private void SetBestColorFamilyToReduce(IOctreeNode newFamily)
        {
            if (newFamily == null) return;
            if (BestColorFamilyToReduce == null || BestColorFamilyToReduce.Index < newFamily.Index)
            {
                BestColorFamilyToReduce = newFamily;
            }
            else if (newFamily.Index == BestColorFamilyToReduce.Index)
            {
                if (newFamily.GetColorPixelsCount() < BestColorFamilyToReduce.GetColorPixelsCount())
                {
                    BestColorFamilyToReduce = newFamily;
                }
            }
        }   
    }

    public class OctreeLeaf : IOctreeNode
    {
        private Color color;
        private int pixelsCount;

        public IOctreeNode BestColorFamilyToReduce { get { return null; } }
        public int Index { get; private set; }

        public OctreeLeaf(Color color, int count = 1, int index = 8)
        {
            Index = index;
            this.color = color;
            pixelsCount = count;
        }

        public Color GetAvgColor()
        {
            return this.color;
        }

        public Color GetColor(Color color)
        {
            return this.color;
        }

        public int GetColorPixelsCount()
        {
            return pixelsCount;
        }

        (int newColorsCount, IOctreeNode bestColorFamilyToReduce) IOctreeNode.AddColor(Color color, int index)
        {
            pixelsCount++;
            return (0,null);
        }

        public void ReduceColorsCount(IOctreeNode nodeToReduce, Color avgFamilyColor)
        {
           
        }

        public int GetColorsCount()
        {
            return 1;
        }
    }
}
