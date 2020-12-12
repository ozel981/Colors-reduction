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
        private Size startSize;

        private ReducedColorsBitmapFactory reducedAfterFactory;
        private ReducedColorsBitmapFactory reducedAlongFactory;
        public Form1()
        {
            InitializeComponent();
            imagePath = Application.StartupPath;
            imagePath = Path.Combine(imagePath.Substring(0, imagePath.Length - 9), @"Images");
            imagePath += "/lena.jpg";
            startSize = (new Bitmap(imagePath)).Size;
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
            DrawReducedAlongBitmap(colorsLimit, pictureBoxAlongSize);
            AlongStatus.Text = "Ready";
            pictureBoxAlong.Refresh();
            DrawReducedAfterBitmap(colorsLimit, pictureBoxAfterSize);
            AfterStatus.Text = "Ready";
            pictureBoxAfter.Refresh();            
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
                    startSize = (new Bitmap(imagePath)).Size;
                    reducedAfterFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath));
                    reducedAlongFactory = new ReducedColorsBitmapFactory(new Bitmap(imagePath), true);
                    RefreshPictureBoxes();
                }
            }
        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }
        //((Image)reducedAfterFactory.GetBitmap(colorsLimit, startSize)).Save(saveFileDialog1.FileName);
        private void SaveAfter_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        ((Image)reducedAfterFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        ((Image)reducedAfterFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        ((Image)reducedAfterFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }
        //((Image)reducedAlongFactory.GetBitmap(colorsLimit, startSize)).Save(saveFileDialog1.FileName);
        private void SaveAlong_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        ((Image)reducedAlongFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        ((Image)reducedAlongFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        ((Image)reducedAlongFactory.GetBitmap(colorsLimit, startSize)).Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }

        }

        private void ReduceToGrayScaleButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(imagePath);
            for (int i = 0; i < bitmap.Width;i++)
            {
                for(int j=0;j<bitmap.Height;j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    double gray = Math.Min(255,Math.Max(0,(0.3 * pixelColor.R) + (0.59 * pixelColor.G) + (0.11 * pixelColor.B)));
                    bitmap.SetPixel(i,j, Color.FromArgb((int)gray, (int)gray, (int)gray) );
                }
            }
            reducedAfterFactory = new ReducedColorsBitmapFactory(bitmap);
            reducedAlongFactory = new ReducedColorsBitmapFactory(bitmap, true);
            pictureBoxAfter.Image = new Bitmap(pictureBoxAfter.Width, pictureBoxAfter.Height);
            pictureBoxAlong.Image = new Bitmap(pictureBoxAlong.Width, pictureBoxAlong.Height);
            pictureBox.Image = new Bitmap(bitmap, pictureBox.Width, pictureBox.Height);
            RefreshPictures();
        }
    }
}
