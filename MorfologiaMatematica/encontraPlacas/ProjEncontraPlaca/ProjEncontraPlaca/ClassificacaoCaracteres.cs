using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ProjEncontraPlaca
{
    internal class ClassificacaoCaracteres
    {
        private int altura;
        private int largura;
        private int n_classes;
        private int n_dim;
        private int tipo;
        private Classe[] classes;

        public ClassificacaoCaracteres(int altura, int largura, int tipo, char flag)
        {
            this.altura = altura;
            this.largura = largura;
            n_dim = altura * largura;
            this.tipo = tipo;

            if (tipo == 1)
            {
                String dig_numeros = "0123456789";
                n_classes = 10;
                classes = new Classe[n_classes];
                for (int i = 0; i < n_classes; i++)
                    classes[i] = new Classe(dig_numeros[i], n_dim);
                if (flag == 'S')
                    monta_arq_aprendizado(dig_numeros, @"..\..\..\numeros.png", @"..\..\..\numeros.txt");
                inicializa_classificador_2pixels(@"..\..\..\numeros.txt");
            }
            else
            {
                String dig_letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                n_classes = 26;
                classes = new Classe[n_classes];
                for (int i = 0; i < n_classes; i++)
                    classes[i] = new Classe(dig_letras[i], n_dim);
                if (flag == 'S')
                    monta_arq_aprendizado(dig_letras, @"..\..\..\letras.png", @"..\..\..\letras.txt");
                inicializa_classificador_2pixels(@"..\..\..\letras.txt");
            }
        }

        public void monta_arq_aprendizado(String digitos, String nome_img, String nome_arq)
        {
            Image img = Image.FromFile(nome_img);
            String transicao;
            int x, y, w, h;
            //int xi, xf, yi, yf;
            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();
            try
            {
                StreamWriter arq = new StreamWriter(nome_arq);

                Bitmap imageBitmap = (Bitmap)img.Clone();
                Filtros.threshold((Bitmap)img, imageBitmap);
                Bitmap imagePB = (Bitmap)imageBitmap.Clone();

                Bitmap imageBitmapDest = new Bitmap(img.Width, img.Height);
                Filtros.segmentar8conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);

                for (int i = 0; i < listaPini.Count; i++)
                {
                    x = listaPini[i].X;
                    y = listaPini[i].Y;
                    w = listaPfim[i].X - listaPini[i].X;
                    h = listaPfim[i].Y - listaPini[i].Y;
                    if (h > 30 && h < 40)
                    {
                        //segmenta o digito
                        Bitmap img_dig = segmentaRoI(imagePB, x, y, w, h);

                        transicao = retornaTransicaoHorizontal(img_dig);
                        arq.WriteLine(digitos[i] + "|" + transicao + "|");
                    }
                }
                arq.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static Bitmap segmentaRoI(Bitmap imageBitmap, int x, int y, int w, int h)
        {
            Bitmap img_dig = new Bitmap(w, h);
            int cor;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    cor = imageBitmap.GetPixel(x + j, y + i).R;
                    img_dig.SetPixel(j, i, Color.FromArgb(cor, cor, cor));
                }
            return img_dig;
        }

        public void inicializa_classificador_2pixels(String nome_arq)
        {
            String linha;
            try
            {
                StreamReader arq = new StreamReader(nome_arq);
                int pos = 0;
                linha = arq.ReadLine();
                while (linha != null)
                {
                    classes[pos].n_restricoes = (n_dim - 1) * 4;
                    String[] lista = linha.Split('|');
                    String transicao = lista[1];
                    for (int j = 0; j < n_dim - 1; j++)
                    {
                        if (transicao[j] == '0' && transicao[j + 1] == '0' && classes[pos].NOC[j]._00 == 1)
                        {
                            classes[pos].NOC[j]._00 = 0;
                            classes[pos].n_restricoes--;
                        }
                        if (transicao[j] == '0' && transicao[j + 1] == '1' && classes[pos].NOC[j]._01 == 1)
                        {
                            classes[pos].NOC[j]._01 = 0;
                            classes[pos].n_restricoes--;
                        }
                        if (transicao[j] == '1' && transicao[j + 1] == '0' && classes[pos].NOC[j]._10 == 1)
                        {
                            classes[pos].NOC[j]._10 = 0;
                            classes[pos].n_restricoes--;
                        }
                        if (transicao[j] == '1' && transicao[j + 1] == '1' && classes[pos].NOC[j]._11 == 1)
                        {
                            classes[pos].NOC[j]._11 = 0;
                            classes[pos].n_restricoes--;
                        }
                    }
                    pos++;
                    linha = arq.ReadLine();
                }
                arq.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public char reconheceCaractereTransicao_2pixels(String transicao)
        {
            int[] cont_NOC = new int[n_classes];
            char caractere = ' ';

            for (int i = 0; i < n_classes; i++)
            {
                cont_NOC[i] = 0;
                for (int j = 0; j < n_dim - 1; j++)
                {
                    // se tem alguma restricao
                    if (classes[i].NOC[j]._00 == 1 || classes[i].NOC[j]._01 == 1 || classes[i].NOC[j]._10 == 1 || classes[i].NOC[j]._11 == 1)
                    {
                        if (transicao[j] == '0' && transicao[j + 1] == '0' && classes[i].NOC[j]._00 == 1)
                            cont_NOC[i]++;
                        if (transicao[j] == '0' && transicao[j + 1] == '1' && classes[i].NOC[j]._01 == 1)
                            cont_NOC[i]++;
                        if (transicao[j] == '1' && transicao[j + 1] == '0' && classes[i].NOC[j]._10 == 1)
                            cont_NOC[i]++;
                        if (transicao[j] == '1' && transicao[j + 1] == '1' && classes[i].NOC[j]._11 == 1)
                            cont_NOC[i]++;
                    }
                }
            }
            int menor = n_dim;
            int pos = 0;
            for (int i = 0; i < n_classes; i++)
                if (cont_NOC[i] < menor)
                {
                    menor = cont_NOC[i];
                    pos = i;
                }

            // se empatou, verificar a classe com mais restricoes,
            // mais restritiva, pois as classes mais exigentes têm prioridade
            int maior = 0;
            for (int i = 0; i < n_classes; i++)
            {
                if (menor == cont_NOC[i])
                    if (classes[i].n_restricoes > maior)
                    {
                        maior = classes[i].n_restricoes;
                        pos = i;
                    }
            }

            if (menor < n_dim)
                caractere = classes[pos].caractere;
            return caractere;
        }

        public String retornaTransicaoHorizontal(Bitmap img)
        {
            //reamostrar a imagem na grade
            Bitmap img_res = new Bitmap(img, new Size(largura, altura));
            Filtros.threshold((Bitmap)img_res, img_res);

            String transicao = String.Empty;
            bool flag = true;

            for (int i = 0; i < altura; i++)
            {
                if (flag)
                {
                    for (int j = 0; j < largura; j++)
                        if (img_res.GetPixel(j, i).R == 0)
                            transicao += '0';
                        else
                            transicao += '1';
                }
                else
                {
                    for (int j = largura - 1; j >= 0; j--)
                        if (img_res.GetPixel(j, i).R == 0)
                            transicao += '0';
                        else
                            transicao += '1';
                }
                flag = !flag;
            }
            return transicao;
        }
    }
}