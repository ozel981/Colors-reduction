using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colors_reduction.OctreeTree
{
    public class Octree
    {
        private IOctreeNode root;
        public int ColorsCount { get; private set; }

        public Octree()
        {
            root = new OctreeNode(0);
            ColorsCount = 0;
        }

        public void AddColor(Color color)
        {
            ColorsCount += root.AddColor(color, 0).newColorsCount;
        }

        public Color GetColor(Color color)
        {
            return root.GetColor(color);
        }

        public void ReduceColorsCount(int limit)
        {
            
            while (ColorsCount > limit)
            {
                if (root == root.BestColorFamilyToReduce)
                {
                    ColorsCount = 1;
                    root = new OctreeLeaf(root.GetAvgColor(), root.GetColorPixelsCount(), 0);
                    return;
                }
                else
                {
                    ColorsCount -= (root.BestColorFamilyToReduce.GetColorsCount() - 1);
                    root.ReduceColorsCount(root.BestColorFamilyToReduce, root.BestColorFamilyToReduce.GetAvgColor());
                }
            }
        }

    }
}


