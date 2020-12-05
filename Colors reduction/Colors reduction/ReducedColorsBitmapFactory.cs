using Colors_reduction.OctreeTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colors_reduction
{
    class ReducedColorsBitmapFactory
    {
        private Bitmap patternBitmap;
        private Octree octree;
        private Size lastCalculatedBitmapSize;
        private bool ReduceColorsAlongCreatingOctree;
        public ReducedColorsBitmapFactory(Bitmap patternBitmap, bool reduceColorsAlongCreatingBitmap = false)
        {
            octree = null;
            this.patternBitmap = patternBitmap;
            lastCalculatedBitmapSize = patternBitmap.Size;
            ReduceColorsAlongCreatingOctree = reduceColorsAlongCreatingBitmap;
        }

        public Bitmap GetBitmap(int colorsLimit, Size bitmapSize)
        {
            Bitmap bitmap = new Bitmap(patternBitmap, bitmapSize);
            
            if( octree != null &&
                bitmapSize == lastCalculatedBitmapSize && 
                octree.ColorsCount >= colorsLimit && 
                !ReduceColorsAlongCreatingOctree)
            {
                octree.ReduceColorsCount(colorsLimit);
                return GetReducedBitmap(bitmap);
            }
            lastCalculatedBitmapSize = bitmapSize;
            NewOctree(bitmap, colorsLimit);
            octree.ReduceColorsCount(colorsLimit);
            return GetReducedBitmap(bitmap);
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

        private Bitmap GetReducedBitmap(Bitmap bitmap)
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
