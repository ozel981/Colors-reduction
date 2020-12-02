using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Colors_reduction.OctreeTree;

namespace Colors_reduction
{
    public partial class Form1 : Form
    {
        private string imagePath;
        private ReducedColorsPicture PictureReduceAfterCreatingOctree;
        private ReducedColorsPicture PictureReduceAlongCreatingOctree;
        public Form1()
        {
            InitializeComponent();
            imagePath = Application.StartupPath;
            imagePath = Path.Combine(imagePath.Substring(0, imagePath.Length - 9), @"Images");
            imagePath += "/lena.jpg";
            pictureBox.Image = new Bitmap(new Bitmap(imagePath), pictureBox.Width, pictureBox.Height);
            pictureBoxAfter.Image = new Bitmap(pictureBoxAfter.Width, pictureBoxAfter.Height);
            pictureBoxAlong.Image = new Bitmap(pictureBoxAlong.Width, pictureBoxAlong.Height);
            PictureReduceAfterCreatingOctree = new ReducedColorsPicture(new Bitmap(imagePath));
            PictureReduceAlongCreatingOctree = new ReducedColorsPicture(new Bitmap(imagePath),true);
        }

        private void DrawPictureWithReducedColorsAfterCreatigOctree()
        {
            using (Graphics graphics = Graphics.FromImage(pictureBoxAfter.Image))
            {
                //TODO: write drawing for reduced colors bitmap
            }
        }

        private void DrawPictureWithReducedColorsAlongCreatigOctree()
        {
            using (Graphics graphics = Graphics.FromImage(pictureBoxAlong.Image))
            {
                //TODO: write drawing for reduced colors bitmap
            }
        }


    }
}
