using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ProjEncontraPlaca
{
    class Filtros
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
            for (int x = menor.X; x <= maior.X; x++)
            {
                imageBitmapDest.SetPixel(x, menor.Y, cor);
                imageBitmapDest.SetPixel(x, maior.Y, cor);
            }
            for (int y = menor.Y; y <= maior.Y; y++)
            {
                imageBitmapDest.SetPixel(menor.X, y, cor);
                imageBitmapDest.SetPixel(maior.X, y, cor);
            }
        }


        //----------------
        public static void encontra_placa(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();

            Otsu otsu = new Otsu();

            otsu.Convert2GrayScaleFast(imageBitmapDest);
            int otsuThreshold = otsu.getOtsuThreshold((Bitmap)imageBitmapDest);
            otsu.threshold(imageBitmapDest, otsuThreshold);


            Bitmap imageBitmap = (Bitmap)imageBitmapDest.Clone();
            Filtros.segmentar8conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);
            //pictBoxImg.Image = imgDest;


            //
            int altura, largura;
            List<Point> _listaPini = new List<Point>();
            List<Point> _listaPfim = new List<Point>();
            for (int i = 0; i < listaPini.Count; i++)
            {
                altura = listaPfim[i].Y - listaPini[i].Y;
                largura = listaPfim[i].X - listaPini[i].X;

                if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                {
                    _listaPini.Add(listaPini[i]);
                    _listaPfim.Add(listaPfim[i]);
                    Filtros.desenhaRetangulo(imageBitmapDest, listaPini[i], listaPfim[i], Color.FromArgb(0, 255, 0));
                }

            }
        }

        //-----

      




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


       /* public static void contorna_branco(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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
        }*/


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
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    if (gs > 220)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }



/*
        public static void afinamento(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            brancoPreto(imageBitmapSrc, imageBitmapSrc);
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            int conectividade, vizinhos;
            bool continuar = true;
            List<List<int>> iteracao = new List<List<int>>();
            List<Tuple<int, int>> pontosDel = new List<Tuple<int, int>>();

            // percorrer imagem
            for (int y = 0; y < height; y++)
            {

                iteracao.Add(new List<int>());

                for (int x = 0; x < width; x++)
                {
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    if (cor.R == 255)
                        iteracao[y].Add(0);
                    else if (cor.R == 0)
                        iteracao[y].Add(1);
                }
            }

            while (continuar)
            {
                continuar = false;
                // primeira sub iteracao
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        vizinhos = 0;
                        conectividade = 0;
                        if (iteracao[y][x] != 0)
                        {
                            conectividade += (iteracao[y - 1][x] == 0 && iteracao[y - 1][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y - 1][x + 1] == 0 && iteracao[y][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y][x + 1] == 0 && iteracao[y + 1][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x + 1] == 0 && iteracao[y + 1][x] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x] == 0 && iteracao[y + 1][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x - 1] == 0 && iteracao[y][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y][x - 1] == 0 && iteracao[y - 1][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y - 1][x - 1] == 0 && iteracao[y - 1][x] == 1) ? 1 : 0;
                            if (conectividade == 1)
                            {
                                vizinhos = iteracao[y - 1][x] + iteracao[y - 1][x + 1] + iteracao[y][x + 1] + iteracao[y + 1][x + 1] + iteracao[y + 1][x]
                                    + iteracao[y + 1][x - 1] + iteracao[y][x - 1] + iteracao[y - 1][x - 1];

                                if (vizinhos > 2 && vizinhos < 6)
                                {
                                    vizinhos = 0;
                                    vizinhos = iteracao[y - 1][x] * iteracao[y][x + 1] * iteracao[y][x - 1];

                                    if (vizinhos == 0)
                                    {
                                        vizinhos = 0;
                                        vizinhos = iteracao[y - 1][x] * iteracao[y + 1][x] * iteracao[y][x - 1];

                                        if (vizinhos == 0)
                                        {
                                            continuar = true;
                                            pontosDel.Add(Tuple.Create(x, y));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < pontosDel.Count; i++)
                    iteracao[pontosDel[i].Item2][pontosDel[i].Item1] = 0;
                pontosDel.Clear();

                //segunda sub iteracao
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        vizinhos = 0;
                        conectividade = 0;
                        if (iteracao[y][x] != 0)
                        {
                            conectividade += (iteracao[y - 1][x] == 0 && iteracao[y - 1][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y - 1][x + 1] == 0 && iteracao[y][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y][x + 1] == 0 && iteracao[y + 1][x + 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x + 1] == 0 && iteracao[y + 1][x] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x] == 0 && iteracao[y + 1][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y + 1][x - 1] == 0 && iteracao[y][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y][x - 1] == 0 && iteracao[y - 1][x - 1] == 1) ? 1 : 0;
                            conectividade += (iteracao[y - 1][x - 1] == 0 && iteracao[y - 1][x] == 1) ? 1 : 0;
                            if (conectividade == 1)
                            {
                                vizinhos = iteracao[y - 1][x] + iteracao[y - 1][x + 1] + iteracao[y][x + 1] + iteracao[y + 1][x + 1] + iteracao[y + 1][x]
                                    + iteracao[y + 1][x - 1] + iteracao[y][x - 1] + iteracao[y - 1][x - 1];

                                if (vizinhos > 2 && vizinhos < 6)
                                {
                                    vizinhos = 0;
                                    vizinhos = iteracao[y - 1][x] * iteracao[y][x + 1] * iteracao[y + 1][x];

                                    if (vizinhos == 0)
                                    {
                                        vizinhos = 0;
                                        vizinhos = iteracao[y][x + 1] * iteracao[y + 1][x] * iteracao[y][x - 1];

                                        if (vizinhos == 0)
                                        {
                                            continuar = true;
                                            pontosDel.Add(Tuple.Create(x, y));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < pontosDel.Count; i++)
                    iteracao[pontosDel[i].Item2][pontosDel[i].Item1] = 0;
                pontosDel.Clear();
            }


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (iteracao[y][x] == 0)
                        imageBitmapDest.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    else
                        imageBitmapDest.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }

            }
        }

        public static void contorno(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int x, y, x2, y2, aux_cR, me_x, ma_x, me_y, ma_y;
            bool flag;

            Color corB = Color.FromArgb(255, 255, 255);
            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                    imageBitmapDest.SetPixel(x, y, corB);

            for (y = 1; y < height; y+=2)
            {
                for (x = 0; x < width - 1; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color cor2 = imageBitmapSrc.GetPixel(x + 1, y);
                    if (cor.R == 255 && cor2.R == 0 && imageBitmapDest.GetPixel(x + 1, y).R == 255)
                    {
                        me_x = ma_x = x2 = x;
                        me_y = ma_y = y2 = y;
                        do
                        {
                            Color p0, p1, p2, p3, p4, p5, p6, p7;
                            imageBitmapDest.SetPixel(x2, y2, Color.FromArgb(0, 0, 0));

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
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0)
                            {
                                x2 = x2 + 1;
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0)
                                x2 = x2 + 1;
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0)
                            {
                                y2 = y2 - 1;
                            }
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0)
                                y2 = y2 - 1;
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0)
                            {
                                x2 = x2 - 1;
                            }
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0)
                                x2 = x2 - 1;
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0)
                            {
                                y2 = y2 + 1;
                            }
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0)
                                y2 = y2 + 1;


                            //menor e maior
                            if (x2 < me_x)
                                me_x = x2;
                            if (y2 < me_y)
                                me_y = y2;
                            if (x2 > ma_x)
                                ma_x = x2;
                            if (y2 > ma_y)
                                ma_y = y2;

                        }
                        while (x != x2 || y != y2);

                        //desenha retângulo mínimo
                        desenhaRetangulo(imageBitmapDest, new Point(me_x, me_y), new Point(ma_x, ma_y), Color.FromArgb(255, 0, 0));
                    }
                }
            }
        }*/
    }
}
