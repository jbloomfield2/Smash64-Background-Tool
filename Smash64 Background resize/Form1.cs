using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Smash64_Background_resize
{
    
    public partial class Form1 : Form
    {
        string filePath;
        Image<Bgr, byte> img1;
        Image<Bgr, byte> img2;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* img2 is a 300x220 copy of the input image
             * img1 is 300x263, and will be the final image after rows are copied
             */
            img1 = new Image<Bgr, byte>(filePath);
            img2 = new Image<Bgr, byte>(300, 220);

            // if resize input width to 220
            if (img1.Height == 263 && img1.Width == 300)
            {
                img2 = img1.Resize(300, 220, Emgu.CV.CvEnum.Inter.Nearest);
            }

            // for 220 width input, make a 263 width copy
            else if (img1.Height == 220)
            {
                Size size1 = new Size(300, 263);
                img2 = new Image<Bgr, byte>(filePath);
                img1 = img2.Resize(300, 263, Emgu.CV.CvEnum.Inter.Nearest);
            }

            // basic size check 
            else
            {
                DialogResult result;
                result = MessageBox.Show("Invalid Image size, Supported sizes are 300x220 and 300x263", "Invalid Image", MessageBoxButtons.OK);
                return;
            }

            // copy first 6 rows, row 7 is a duplicate
            for (int i = 0; i < 7; i++)
                copyRow(i, i);
            copyRow(6, 7);

            int count = 0;
            int row = 0;

            // from row 8, every 5th row is a duplicate
            for (int i = 8;i <221; i++)
            {
                if (i + row > 263)
                    break;
                copyRow(i -1, i+row);
                count++;
                if(count == 5 && i <263)
                {
                    row++;
                    copyRow(i-1, i+ row);
                    count = 0;
                }


            }


            imageBox1.Image = img1;

        }

        // copies a row of pixels, one at a time from one image to another
        private void copyRow(int rowSource, int rowDest)
        {
            for (int i = 0; i < 300; i++)
            {
                img1[rowDest, i] = img2[rowSource, i];
            }
        }

        // load and display file
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Png File (*.png)|*.png| Bmp file (*.bmp)|*.bmp";

            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
                filePath = openFileDialog1.FileName;
                imageBox1.Image = CvInvoke.Imread(filePath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            }
        }

        // open save dialog
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog1 = new SaveFileDialog();
            Dialog1.Filter = "Png file (*.png)|*.png|Bmp file (*.bmp)|*.bmp";
            Dialog1.RestoreDirectory = true;

            if(Dialog1.ShowDialog() ==DialogResult.OK)
            {
                img1.Save(Dialog1.FileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
