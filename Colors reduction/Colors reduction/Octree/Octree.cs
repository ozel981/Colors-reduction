using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction.Octree
{
    class Octree
    {
        private IOctreeNode root;
        public int ColorsCount { get; private set; }

        public Octree()
        {
            root = new OctreeNode();
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
            if (ColorsCount <= limit) return;
            // TODO: write function
        }
    }
}
