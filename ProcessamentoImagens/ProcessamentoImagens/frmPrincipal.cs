using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ProcessamentoImagens
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnAbrirImagem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg1.Image = image;
                pictBoxImg1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg1.Image = null;
            pictBoxImg2.Image = null;
        }

        private void btnLuminanciaSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_gray(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnLuminanciaComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_grayDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativo(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativoDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

  

        private void espelhoVerticalDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.verticalSemDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void espelhoVsemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.verticalSemDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void espelhoVDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.espelhoVeticalDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void espelhoHsemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.horizontalSemDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void espelhoHDMA_Click(object sender, EventArgs e)
        {

        }

        private void ZhangSuenComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.ZhangSuen(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }
    }
}
