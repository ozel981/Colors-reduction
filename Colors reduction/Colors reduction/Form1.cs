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
        private int colorsLimit = 1;

        private ReducedColorsBitmapFactory reducedAfterFactory;
        private ReducedColorsBitmapFactory reducedAlongFactory;
        public Form1()
        {
            InitializeComponent();
            imagePath = Application.StartupPath;
            imagePath = Path.Combine(imagePath.Substring(0, imagePath.Length - 9), @"Images");
            imagePath += "/lena.jpg";
            pictureBox.Image = new Bitmap(new Bitmap(imagePath), pictureBox.Width, pictureBox.Height);
            pictureBoxAfter.Image = new Bitmap(pictureBoxAfter.Width, pictureBoxAfter.Height);
            pictureBoxAlong.Image = new Bitmap(pictureBoxAlong.Width, pictureBoxAlong.Height);
            reducedAfterFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath));
            reducedAlongFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath),true);
        }

        private void DrawReducedAfterBitmap(int colorsLimit, Size pictureBoxSize)
        {
            using (Graphics graphics = Graphics.FromImage(pictureBoxAfter.Image))
            {
                graphics.DrawImage(reducedAfterFactory.GetBitmap(colorsLimit, pictureBoxSize), new Point(0, 0));
            }
        }

        private void DrawReducedAlongBitmap(int colorsLimit, Size pictureBoxSize)
        {
            using (Graphics graphics = Graphics.FromImage(pictureBoxAlong.Image))
            {
                graphics.DrawImage(reducedAlongFactory.GetBitmap(colorsLimit, pictureBoxSize), new Point(0, 0));
            }
        }

        private void ColorsLimit_Scroll(object sender, EventArgs e)
        {
            colorsLimit = ((TrackBar)sender).Value;
            if(colorsLimit == 1) ReduceColorsButton.Text = $"Reduce to {colorsLimit} color";
            else ReduceColorsButton.Text = $"Reduce to {colorsLimit} colors";
        }

        private void RefreshPictures()
        {
            AfterStatus.Text = "Calculating...";
            AlongStatus.Text = "Calculating...";
            this.Refresh();
            Size pictureBoxAlongSize = pictureBoxAlong.Size;
            Size pictureBoxAfterSize = pictureBoxAfter.Size;
            Task alongTask = new Task(() => DrawReducedAlongBitmap(colorsLimit, pictureBoxAlongSize));

            Task afterTask = new Task(() => DrawReducedAfterBitmap(colorsLimit, pictureBoxAfterSize));

            alongTask.Start();
            afterTask.Start();

            Task.WaitAny(alongTask, afterTask);

            if (alongTask.IsCompleted)
            {
                AlongStatus.Text = "Ready";
                pictureBoxAlong.Refresh();
            }
            if (afterTask.IsCompleted)
            {
                AfterStatus.Text = "Ready";
                pictureBoxAfter.Refresh();
            }

            Task.WaitAll(alongTask, afterTask);

            if (alongTask.IsCompleted)
            {
                AlongStatus.Text = "Ready";
                pictureBoxAlong.Refresh();
            }
            if (afterTask.IsCompleted)
            {
                AfterStatus.Text = "Ready";
                pictureBoxAfter.Refresh();
            }
        }

        private void ReduceColorsButton_Click(object sender, EventArgs e)
        {
            RefreshPictures();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            RefreshPictureBoxes();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ColorsLimit.Maximum = (int)((NumericUpDown)sender).Value;
        }

        private void RefreshPictureBoxes()
        {
            pictureBoxAfter.Image = new Bitmap(pictureBoxAfter.Width, pictureBoxAfter.Height);
            pictureBoxAlong.Image = new Bitmap(pictureBoxAlong.Width, pictureBoxAlong.Height);
            pictureBox.Image = new Bitmap(new Bitmap(imagePath), pictureBox.Width, pictureBox.Height);
            RefreshPictures();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

            RefreshPictureBoxes();
        }


        private void LoadBitmapButton_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string path = Application.StartupPath;
                openFileDialog.InitialDirectory = Path.Combine(path.Substring(0, path.Length - 9), @"Images");
                openFileDialog.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName;
                    reducedAfterFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath));
                    reducedAlongFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath), true);
                    RefreshPictureBoxes();
                }
            }
        }
    }
}
