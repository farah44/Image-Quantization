using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public static int k;
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
           
            int distinc=construct.distinctColor(ImageMatrix);
            textBoxK.Text = distinc.ToString();
             double mst= DistanceCalculators.DistanceCalculator(construct.distinct);
             textBoxMST.Text = mst.ToString();
            clustering.phaseOne(DistanceCalculators.distances, k);
            clustering.phaseTwo(DistanceCalculators.parent, clustering.disconnected);
            int[] arr = clustering.phaseTwo(DistanceCalculators.parent, clustering.disconnected);
            clustering.graph(arr, clustering.disconnected);
            clustering.phaseThree(clustering.g, k);
            clustering.AVG(k);
            clustering.Gausssmooth(ImageMatrix);
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
          
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void KButton_Click(object sender, EventArgs e)
        {
           k = int.Parse(KText.Text);
        }

        private void textBoxK_TextChanged(object sender, EventArgs e)
        {

        }

       
       
    }
}