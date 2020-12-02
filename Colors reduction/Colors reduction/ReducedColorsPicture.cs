using Colors_reduction.OctreeTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction
{
    class ReducedColorsPicture
    {
        private Bitmap OrginalBitmap;
        private Octree octree;
        private Size BitmapSize;
        private bool ReduceColorsAlongCreatingOctree;
        public ReducedColorsPicture(Bitmap bitmap, bool reduceColorsAlongCreatingOctree = false)
        {
            octree = null;
            OrginalBitmap = bitmap;
            BitmapSize = OrginalBitmap.Size;
            ReduceColorsAlongCreatingOctree = reduceColorsAlongCreatingOctree;
        }

        public Bitmap CalculateReducedColorsBitmap(int colorsLimit, Size bitmapSize)
        {
            Bitmap bitmap = new Bitmap(OrginalBitmap, bitmapSize);
            if(bitmapSize == BitmapSize && octree != null && octree.ColorsCount >= colorsLimit && !ReduceColorsAlongCreatingOctree)
            {
                octree.ReduceColorsCount(colorsLimit);
                return ReducedColors(bitmap);
            }
            BitmapSize = bitmapSize;
            NewOctree(bitmap, colorsLimit);
            octree.ReduceColorsCount(colorsLimit);
            return ReducedColors(bitmap);
        }

        private void NewOctree(Bitmap bitmap, int colorsLimit)
        {
            octree = new Octree();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    octree.AddColor(bitmap.GetPixel(i, j));
                    if(ReduceColorsAlongCreatingOctree)
                    {
                        octree.ReduceColorsCount(colorsLimit);
                    }
                }
            }
        }

        private Bitmap ReducedColors(Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    bitmap.SetPixel(i, j, octree.GetColor(bitmap.GetPixel(i, j)));
                }
            }
            return bitmap;
        }
    }
}
