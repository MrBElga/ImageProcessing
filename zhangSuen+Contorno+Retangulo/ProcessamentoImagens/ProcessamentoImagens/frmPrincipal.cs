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
                pictBoxImg1.SizeMode = PictureBoxSizeMode.Zoom;
                pictBoxImg2.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }   

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg1.Image = null;
            pictBoxImg2.Image = null;
        }

        private void ZhangSuenComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.pretoeBranco(imageBitmap, imgDest);
            imageBitmap = imgDest;
       
            pictBoxImg1.Image = imageBitmap;
            imgDest = new Bitmap(imageBitmap);
            Filtros.ZhangSuen(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            imgDest.Save("afina.png",ImageFormat.Png);
        }

        private void BordaSDma_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.pretoeBranco(imageBitmap, imgDest);
            imageBitmap = imgDest;

            pictBoxImg1.Image = imageBitmap;
            imgDest = new Bitmap(imageBitmap);
            Filtros.ZhangSuen(imageBitmap, imgDest);
            imageBitmap = new Bitmap(imgDest);
            pictBoxImg1.Image = imageBitmap;

            imgDest = new Bitmap(imageBitmap.Width, imageBitmap.Height);
            using (Graphics g = Graphics.FromImage(imgDest))
            {
                g.Clear(Color.White); 
            }
            Filtros.borda(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            imgDest.Save("borda.png", ImageFormat.Png);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
