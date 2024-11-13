using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjEncontraPlaca
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;
        private Otsu otsu;

        public frmPrincipal()
        {
            InitializeComponent();
            otsu = new Otsu();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.png)|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg.Image = image;
                pictBoxImg.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg.Image = null;
            textBox1.Text = "";
        }

        private void btnOTSU_Click(object sender, EventArgs e)
        {
            Bitmap temp = (Bitmap)image.Clone();
            otsu.Convert2GrayScaleFast(temp);
            int otsuThreshold = otsu.getOtsuThreshold((Bitmap)temp);
            otsu.threshold(temp, otsuThreshold);
            textBox1.Text = otsuThreshold.ToString();
            pictBoxImg.Image = temp;
        }

        private void btnSegmenta8_Click(object sender, EventArgs e)
        {
            imageBitmap = (Bitmap)image.Clone();
            Bitmap imgDest = (Bitmap)image.Clone();
            Filtros.encontra_placa(imageBitmap, imgDest);

            pictBoxImg.Image = imgDest;
        }

        private void btnReconheDigito_Click(object sender, EventArgs e)
        {
            ClassificacaoCaracteres cl_numeros = new ClassificacaoCaracteres(30, 40, 1, 'S');
            ClassificacaoCaracteres cl_letras = new ClassificacaoCaracteres(30, 40, 2, 'S');

            //testando o reconhecimento dos caracteres
            Image img = Image.FromFile(@"..\..\..\H.png");
            Bitmap img_dig = new Bitmap(img.Width, img.Height);
            Filtros.threshold((Bitmap)img, img_dig);

            String transicao = cl_letras.retornaTransicaoHorizontal(img_dig);
            Console.WriteLine(cl_letras.reconheceCaractereTransicao_2pixels(transicao));

            //testando o reconhecimento dos numeros
            img = Image.FromFile(@"..\..\..\7.png");
            img_dig = new Bitmap(img.Width, img.Height);
            Filtros.threshold((Bitmap)img, img_dig);

            transicao = cl_numeros.retornaTransicaoHorizontal(img_dig);
            Console.WriteLine(cl_numeros.reconheceCaractereTransicao_2pixels(transicao));
        }
    }
}