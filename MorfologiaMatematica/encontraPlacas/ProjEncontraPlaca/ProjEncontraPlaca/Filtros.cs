using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProjEncontraPlaca
{
    internal class Filtros
    {
        private static void segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
        {
            Point menor = new Point(), maior = new Point(), patual = new Point();
            Queue<Point> fila = new Queue<Point>();
            menor.X = maior.X = ini.X;
            menor.Y = maior.Y = ini.Y;
            fila.Enqueue(ini);
            while (fila.Count != 0)
            {
                patual = fila.Dequeue();
                imageBitmapSrc.SetPixel(patual.X, patual.Y, Color.FromArgb(255, 0, 0));
                imageBitmapDest.SetPixel(patual.X, patual.Y, cor_pintar);

                if (patual.X < menor.X)
                    menor.X = patual.X;
                if (patual.X > maior.X)
                    maior.X = patual.X;
                if (patual.Y < menor.Y)
                    menor.Y = patual.Y;
                if (patual.Y > maior.Y)
                    maior.Y = patual.Y;

                if (patual.X > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X - 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X - 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y - 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y - 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y - 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X < imageBitmapSrc.Width - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.X < imageBitmapSrc.Width - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X + 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X + 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y < imageBitmapSrc.Height - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y < imageBitmapSrc.Height - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y + 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y + 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y + 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
            }

            if (menor.X > 0)
                menor.X--;
            if (maior.X < imageBitmapSrc.Width - 1)
                maior.X++;
            if (menor.Y > 0)
                menor.Y--;
            if (maior.Y < imageBitmapSrc.Height - 1)
                maior.Y++;
            desenhaRetangulo(imageBitmapDest, menor, maior, Color.FromArgb(255, 0, 0));
            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        public static void segmentar8conectado(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    if (r == 0)
                        segmenta8(imageBitmapSrc, imageBitmapDest, new Point(x, y), listaPini, listaPfim, Color.FromArgb(100, 100, 100));
                }
            }
        }

        private static void desenhaRetangulo(Bitmap imageBitmapDest, Point menor, Point maior, Color cor)
        {
            // Garantir que as coordenadas estejam dentro dos limites da imagem
            int width = imageBitmapDest.Width;
            int height = imageBitmapDest.Height;

            menor.X = Math.Max(0, Math.Min(menor.X, width - 1));
            menor.Y = Math.Max(0, Math.Min(menor.Y, height - 1));
            maior.X = Math.Max(0, Math.Min(maior.X, width - 1));
            maior.Y = Math.Max(0, Math.Min(maior.Y, height - 1));

            // Desenhar linhas horizontais (topo e base do retângulo)
            for (int x = menor.X; x <= maior.X; x++)
            {
                if (menor.Y >= 0 && menor.Y < height)
                    imageBitmapDest.SetPixel(x, menor.Y, cor);
                if (maior.Y >= 0 && maior.Y < height)
                    imageBitmapDest.SetPixel(x, maior.Y, cor);
            }

            // Desenhar linhas verticais (esquerda e direita do retângulo)
            for (int y = menor.Y; y <= maior.Y; y++)
            {
                if (menor.X >= 0 && menor.X < width)
                    imageBitmapDest.SetPixel(menor.X, y, cor);
                if (maior.X >= 0 && maior.X < width)
                    imageBitmapDest.SetPixel(maior.X, y, cor);
            }
        }


        private static void reconheceDigito(Bitmap imageBitmapDest, Point inicio, Point fim, ClassificacaoCaracteres cl_numeros, ClassificacaoCaracteres cl_letras)
        {
            int x, y, w, h;
            x = inicio.X;
            y = inicio.Y;
            w = fim.X - inicio.X;
            h = fim.Y - inicio.Y;
            Bitmap img = ClassificacaoCaracteres.segmentaRoI(imageBitmapDest, x, y, w, h);
            Bitmap img_dig = new Bitmap(img.Width, img.Height);
            Filtros.threshold(img, img_dig);
            String transicao;

            transicao = cl_letras.retornaTransicaoHorizontal(img_dig);
            Console.WriteLine(cl_letras.reconheceCaractereTransicao_2pixels(transicao) + "  -  " + cl_numeros.reconheceCaractereTransicao_2pixels(transicao));
        }

        //----------------
        public static void encontra_placa(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            ClassificacaoCaracteres cl_numeros = new ClassificacaoCaracteres(30, 40, 1, 'S');
            ClassificacaoCaracteres cl_letras = new ClassificacaoCaracteres(30, 40, 2, 'S');

            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();

            Otsu otsu = new Otsu();

            otsu.Convert2GrayScaleFast(imageBitmapDest);
            int otsuThreshold = otsu.getOtsuThreshold((Bitmap)imageBitmapDest);
            otsu.threshold(imageBitmapDest, otsuThreshold);

            Bitmap imageBitmap = (Bitmap)imageBitmapDest.Clone();
            Filtros.segmentar8conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);
            //se a lista ter algo ele recorta a placa na imagem original

         
   

            int altura, largura;
            List<Point> _listaPini = new List<Point>();
            List<Point> _listaPfim = new List<Point>();
            int aux=0;
            for (int i = 0; i < listaPini.Count; i++)
            {
                altura = listaPfim[i].Y - listaPini[i].Y;
                largura = listaPfim[i].X - listaPini[i].X;

                if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                {
                    _listaPini.Add(listaPini[i]);
                    _listaPfim.Add(listaPfim[i]);
                    aux++;
                }
            }
            // Verifica se existem regiões válidas
            if (_listaPini.Count > 0)
            {
                // Calcula os limites gerais das regiões válidas
                Point menor = _listaPini[0];
                Point maior = _listaPfim[0];

                for (int i = 1; i < _listaPini.Count; i++)
                {
                    menor.X = Math.Min(menor.X, _listaPini[i].X);
                    menor.Y = Math.Min(menor.Y, _listaPini[i].Y);
                    maior.X = Math.Max(maior.X, _listaPfim[i].X);
                    maior.Y = Math.Max(maior.Y, _listaPfim[i].Y);
                }

                // Expande os limites com margens
                int margem = 20;
                menor.X = Math.Max(0, menor.X - margem);
                menor.Y = Math.Max(0, menor.Y - margem);
                maior.X = Math.Min(imageBitmapSrc.Width - 1, maior.X + margem);
                maior.Y = Math.Min(imageBitmapSrc.Height - 1, maior.Y + margem);

                // Calcula largura e altura finais
                int larguraFinal = maior.X - menor.X + 1;
                int alturaFinal = maior.Y - menor.Y + 1;

                // Recorta a região na imagem original
                Bitmap regiaoRecortada = ClassificacaoCaracteres.segmentaRoI(imageBitmapSrc, menor.X, menor.Y, larguraFinal, alturaFinal);
    
                // Reaplica Otsu na região recortada, se necessário
                otsu.Convert2GrayScaleFast(regiaoRecortada);
                otsuThreshold = otsu.getOtsuThreshold(regiaoRecortada);
                otsu.threshold(regiaoRecortada, otsuThreshold);
                altura =0;
                largura=0;
                _listaPini = new List<Point>();
                _listaPfim = new List<Point>();
                aux = 0;
                for (int i = 0; i < listaPini.Count; i++)
                {
                    altura = listaPfim[i].Y - listaPini[i].Y;
                    largura = listaPfim[i].X - listaPini[i].X;

                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        _listaPini.Add(listaPini[i]);
                        _listaPfim.Add(listaPfim[i]);
                        Filtros.desenhaRetangulo(regiaoRecortada, listaPini[i], listaPfim[i], Color.FromArgb(0, 255, 0));

                        Filtros.reconheceDigito(regiaoRecortada, listaPini[i], listaPfim[i], cl_numeros, cl_letras);
                        aux++;
                    }
                }
            }
        

    }

  

        //sem acesso direto a memoria
        public static void threshold(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.1140 + g * 0.5870 + b * 0.2990);
                    if (gs > 127)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void contorna_branco(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            Color newcolor = Color.FromArgb(255, 255, 255);
            for (int y = 0; y < height; y++)
            {
                for (int col = 0; col < 2; col++)
                {
                    imageBitmapDest.SetPixel(col, y, newcolor);
                    imageBitmapDest.SetPixel(width - 1 - col, y, newcolor);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int lin = 0; lin < 2; lin++)
                {
                    imageBitmapDest.SetPixel(x, lin, newcolor);
                    imageBitmapDest.SetPixel(x, height - 1 - lin, newcolor);
                }
            }
        }

        public static void countour(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int x, y, x2, y2, aux_cR;
            bool flag;

            Color corB = Color.FromArgb(255, 255, 255);
            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                    imageBitmapDest.SetPixel(x, y, corB);

            Bitmap imageBranca = (Bitmap)imageBitmapDest.Clone();

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width - 1; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color cor2 = imageBitmapSrc.GetPixel(x + 1, y);
                    if (cor.R == 255 && cor2.R == 0 && imageBitmapDest.GetPixel(x + 1, y).R == 255)
                    {
                        Bitmap imageAux = (Bitmap)imageBranca.Clone();
                        x2 = x;
                        y2 = y;
                        do
                        {
                            Color p0, p1, p2, p3, p4, p5, p6, p7;
                            imageBitmapDest.SetPixel(x2, y2, Color.FromArgb(0, 0, 0));
                            imageAux.SetPixel(x2, y2, Color.FromArgb(1, 1, 1));

                            p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                            p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                            p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                            p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                            p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                            p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                            p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                            p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);

                            if (p1.R == 255 && p0.R == 255 && p2.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p3.R == 255 && p4.R == 0 && p2.R == 255)
                            {
                                x2 = x2 - 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p5.R == 255 && p4.R == 255 && p6.R == 0)
                            {
                                x2 = x2 - 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p7.R == 255 && p6.R == 255 && p0.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R != 2)
                            {
                                x2 = x2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p0.R == 255 && p2.R == 0 && p1.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R == 2)
                                x2 = x2 + 1;
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R != 2)
                            {
                                y2 = y2 - 1;
                                flag = true;
                                do
                                {
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p2.R == 255 && p4.R == 0 && p3.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R == 2)
                                y2 = y2 - 1;
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R != 2)
                            {
                                x2 = x2 - 1;
                                flag = true;
                                do
                                {
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p4.R == 255 && p5.R == 0 && p6.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R == 2)
                                x2 = x2 - 1;
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R != 2)
                            {
                                y2 = y2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p6.R == 255 && p0.R == 0 && p7.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R == 2)
                                y2 = y2 + 1;
                        }
                        while (x != x2 || y != y2);
                    }
                }
            }
        }

        public static void brancoPreto(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    if (gs > 220)
                        gs = 255;
                    else
                        gs = 0;

                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static Bitmap Dilatacao(Bitmap imageSrc, int[,] mascara, int tamX, int tamY, int[] origem)
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();

            int width = imageSrc.Width;
            int height = imageSrc.Height;
            int mat = mascara.Length;
            Console.WriteLine(mat);
            for (int x = tamX; x < width - tamX; x++)
            {
                for (int y = tamY; y < height - tamY; y++)
                {
                    int R = imageSrc.GetPixel(x, y).R;
                    if (R < 128)
                    {
                        for (int j = 0; j < tamX; j++)
                        {
                            for (int i = 0; i < tamY; i++)
                            {
                                if (mascara[i, j] != 0)
                                {
                                    imageDest.SetPixel(x - origem[0] + j, y - origem[1] + i, Color.Black);
                                }
                            }
                        }
                    }
                }
            }

            return imageDest;
        }

        public static Bitmap Erosao(Bitmap imageSrc, int[,] mascara, int tamX, int tamY, int[] origem)
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();
            int width = imageSrc.Width;
            int height = imageSrc.Height;

            for (int x = origem[0]; x < width - (tamX - origem[0]); x++)
            {
                for (int y = origem[1]; y < height - (tamY - origem[1]); y++)
                {
                    if (imageSrc.GetPixel(x, y).R < 128)
                    {
                        bool flag = false;
                        for (int i = 0; i < tamX && !flag; i++)
                        {
                            for (int j = 0; j < tamY && !flag; j++)
                            {
                                if (mascara[i, j] != 0 && !(i == origem[0] && j == origem[1]))
                                {
                                    if (imageSrc.GetPixel(x - origem[0] + i, y - origem[1] + j).R > 128)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            imageDest.SetPixel(x, y, Color.White);
                        }
                    }
                }
            }
            return imageDest;
        }
    }
}