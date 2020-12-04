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
                graphics.DrawImage(PictureReduceAfterCreatingOctree.CalculateReducedColorsBitmap(ColorsLimit.Value, pictureBoxAfter.Size),new Point(0,0));
            }
            pictureBoxAfter.Refresh();
            AfterStatus.Text = "Ready";
        }

        private void DrawPictureWithReducedColorsAlongCreatigOctree()
        { 
            using (Graphics graphics = Graphics.FromImage(pictureBoxAlong.Image))
            {
                graphics.DrawImage(PictureReduceAlongCreatingOctree.CalculateReducedColorsBitmap(ColorsLimit.Value, pictureBoxAlong.Size), new Point(0, 0));
            }
            pictureBoxAlong.Refresh();
            AlongStatus.Text = "Ready";
        }

        private void ColorsLimit_Scroll(object sender, EventArgs e)
        {
            ReduceColorsButton.Text = $"Reduce to {((TrackBar)sender).Value} colors";
        }

        private void ReduceColorsButton_Click(object sender, EventArgs e)
        {
            AfterStatus.Text = "Calculating...";
            AlongStatus.Text = "Calculating...";
            this.Refresh();
            /*Task afterReduce = new Task(DrawPictureWithReducedColorsAfterCreatigOctree);
            Task alongReduce = new Task(DrawPictureWithReducedColorsAlongCreatigOctree);
            afterReduce.Start();
            alongReduce.Start();*/
            DrawPictureWithReducedColorsAlongCreatigOctree();
            this.Refresh();
            DrawPictureWithReducedColorsAfterCreatigOctree();
            this.Refresh();

        }
    }
}
