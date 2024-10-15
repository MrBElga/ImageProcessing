using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
        /*
        private void btnLuminanciaSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_gray(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }
        */
        private void dilatacao_Click(object sender, EventArgs e)
        {
            // Carregar a imagem original de algum caminho (ou pode ser de um PictureBox)
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            // Chamar o método de dilatação da classe Filtros
            int tamX = 3,tamY = 3;
            int[] origem = new int[2];
            origem[0] = 1;
            origem[1] = 1;
             
            int[,] mascara = new int[tamX, tamY];
            // Defining the mask
            mascara[0, 0] = 0; // Top left
            mascara[0, 1] = 1; // Top center
            mascara[0, 2] = 0; // Top right

            mascara[1, 0] = 1; // Middle left
            mascara[1, 1] = 1; // Middle center
            mascara[1, 2] = 1; // Middle right

            mascara[2, 0] = 0; // Bottom left
            mascara[2, 1] = 1; // Bottom center
            mascara[2, 2] = 0; // Bottom right

            Bitmap imageDest = Filtros.Dilatacao(imageBitmap,mascara, tamX, tamY, origem);

            // Exibir a imagem dilatada em um PictureBox (substitua pictureBoxResultado pelo seu PictureBox)
            pictBoxImg2.Image = imageDest;
            try
            {
                imageDest.Save("caminho_para_imagem_resultado1.png", ImageFormat.Png);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show($"Failed to save image: {ex.Message}");
            }
            // Ou salvar a imagem processada
         
        }

        private void erosao_Click(object sender, EventArgs e)
        {
           
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Bitmap imageDest = Filtros.Erosao(imageBitmap);
            pictBoxImg2.Image = imageDest;
            try
            {
                imageDest.Save("caminho_para_imagem_resultado2.Png", ImageFormat.Png);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show($"Failed to save image: {ex.Message}");
            }
         
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
