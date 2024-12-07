﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjEncontraPlaca
{
    internal class Filtros
    {
        private static readonly Color CorVisitado = Color.FromArgb(255, 0, 0);
        private static readonly Color CorSegmentada = Color.FromArgb(100, 100, 100);

        private static void segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
        {
            Point menor = new Point(ini.X, ini.Y);
            Point maior = new Point(ini.X, ini.Y);
            Queue<Point> fila = new Queue<Point>();
            fila.Enqueue(ini);

            while (fila.Count != 0)
            {
                Point patual = fila.Dequeue();
                imageBitmapSrc.SetPixel(patual.X, patual.Y, CorVisitado);
                imageBitmapDest.SetPixel(patual.X, patual.Y, cor_pintar);

                AtualizaLimites(ref menor, ref maior, patual);

                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X - 1, patual.Y);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X - 1, patual.Y - 1);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X, patual.Y - 1);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X + 1, patual.Y - 1);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X + 1, patual.Y);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X + 1, patual.Y + 1);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X, patual.Y + 1);
                VerificaEAdicionaPonto(imageBitmapSrc, fila, patual.X - 1, patual.Y + 1);
            }

            AjustaLimites(imageBitmapSrc, ref menor, ref maior);
            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        private static void AtualizaLimites(ref Point menor, ref Point maior, Point patual)
        {
            if (patual.X < menor.X) menor.X = patual.X;
            if (patual.X > maior.X) maior.X = patual.X;
            if (patual.Y < menor.Y) menor.Y = patual.Y;
            if (patual.Y > maior.Y) maior.Y = patual.Y;
        }

        private static void VerificaEAdicionaPonto(Bitmap imageBitmapSrc, Queue<Point> fila, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < imageBitmapSrc.Width && y < imageBitmapSrc.Height)
            {
                Color cor = imageBitmapSrc.GetPixel(x, y);
                if (cor.R == 0)
                {
                    fila.Enqueue(new Point(x, y));
                    imageBitmapSrc.SetPixel(x, y, CorVisitado);
                }
            }
        }

        private static void AjustaLimites(Bitmap imageBitmapSrc, ref Point menor, ref Point maior)
        {
            if (menor.X > 0) menor.X--;
            if (maior.X < imageBitmapSrc.Width - 1) maior.X++;
            if (menor.Y > 0) menor.Y--;
            if (maior.Y < imageBitmapSrc.Height - 1) maior.Y++;
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

        public static void encontra_placa(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, PictureBox pictBoxImg)
        {
            ClassificacaoCaracteres cl_numeros = new ClassificacaoCaracteres(30, 40, 1, 'S');
            ClassificacaoCaracteres cl_letras = new ClassificacaoCaracteres(30, 40, 2, 'S');

            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();

            Otsu otsu = new Otsu();

            // Aplica Otsu
            otsu.Convert2GrayScaleFast(imageBitmapDest);
            int otsuThreshold = otsu.getOtsuThreshold((Bitmap)imageBitmapDest);
            otsu.threshold(imageBitmapDest, otsuThreshold);
            // aq segmenta a imagem
            Bitmap imageBitmap = (Bitmap)imageBitmapDest.Clone();
            Filtros.segmentar8conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);

            // lista para achar a placa
            List<Rectangle> possiveisPlacas = new List<Rectangle>();
            int altura = 0, largura = 0;
            Desenha(imageBitmapDest, listaPini, listaPfim, altura, largura, possiveisPlacas);

            // mostra detecção inicial
            pictBoxImg.Image = (Bitmap)imageBitmapDest.Clone();

            // agrupa retangulos afim de achar a  placa
            var (areaPlaca, contagemRetangulos) = AgruparRetangulos(possiveisPlacas, imageBitmapDest);

            if (contagemRetangulos > 0 && contagemRetangulos < 5)
            {
                Console.WriteLine("placa com menos de 4 digitos encontrada!");
                Rectangle areaPlacaAjustada = new Rectangle(
                    Math.Max(0, areaPlaca.X - 166),
                    Math.Max(0, areaPlaca.Y - 10),
                    Math.Min(imageBitmapSrc.Width - areaPlaca.X - 1, areaPlaca.Width + 180),
                    Math.Min(imageBitmapSrc.Height - areaPlaca.Y - 1, areaPlaca.Height + 15)
                );

                Bitmap placaRecortada = RecortarImagem(imageBitmapSrc, areaPlacaAjustada);

                pictBoxImg.Image = placaRecortada;

                listaPini = new List<Point>();
                listaPfim = new List<Point>();

                otsu = new Otsu();

                // Aplica Otsu
                otsu.Convert2GrayScaleFast(placaRecortada);
                otsuThreshold = otsu.getOtsuThreshold((Bitmap)placaRecortada);
                otsu.threshold(placaRecortada, otsuThreshold);

                // Aplica erosão para afinar as letras
         
                // Segmenta a imagem
                Bitmap imageBitmap2 = (Bitmap)placaRecortada.Clone();
                Filtros.segmentar8conectado(imageBitmap2, placaRecortada, listaPini, listaPfim);

                altura = 0;
                largura = 0;
                List<Point> _listaPini = new List<Point>();
                List<Point> _listaPfim = new List<Point>();

                for (int i = 0; i < listaPini.Count; i++)
                {
                    altura = listaPfim[i].Y - listaPini[i].Y;
                    largura = listaPfim[i].X - listaPini[i].X;

                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        // Desenha o retângulo verde na imagem (caractere detectado dentro da placa)
                        _listaPini.Add(listaPini[i]);
                        _listaPfim.Add(listaPfim[i]);
                    }
                }

                for (int i = 0; i < _listaPini.Count; i++)
                {
                    altura = _listaPfim[i].Y - _listaPini[i].Y;
                    largura = _listaPfim[i].X - _listaPini[i].X;

                    // Acha a placa com base no tamanho dos caracteres
                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        Desenha(placaRecortada, _listaPini, _listaPfim, altura, largura, possiveisPlacas);
                        Filtros.reconheceDigito(placaRecortada, _listaPini[i], _listaPfim[i], cl_numeros, cl_letras);
                    }
                }

                pictBoxImg.Image = placaRecortada;
            }
            else if (contagemRetangulos >= 5)
            {
                Console.WriteLine("placa com mais de 4 digitos encontrada!");
                Bitmap imageRecortadaitmap = (Bitmap)imageBitmapSrc.Clone();
                // recortando em volta
                Rectangle placaComMargem = new Rectangle(
                    Math.Max(0, areaPlaca.X - 10),
                    Math.Max(0, areaPlaca.Y - 10),
                    Math.Min(imageRecortadaitmap.Width - areaPlaca.X - 1, areaPlaca.Width + 20),
                    Math.Min(imageRecortadaitmap.Height - areaPlaca.Y - 1, areaPlaca.Height + 20)
                );

                // recorta
                // Bitmap placaRecortada = RecortarImagem(imageRecortadaitmap, placaComMargem);
                Bitmap placaRecortada = (Bitmap)imageBitmapSrc.Clone();

                pictBoxImg.Image = placaRecortada;

                listaPini = new List<Point>();
                listaPfim = new List<Point>();

                otsu = new Otsu();

                // Aplica Otsu
                otsu.Convert2GrayScaleFast(placaRecortada);
                otsuThreshold = otsu.getOtsuThreshold((Bitmap)placaRecortada);
                otsu.threshold(placaRecortada, otsuThreshold);

                // Aplica erosão para afinar as letras
     
          
                // Segmenta a imagem
                Bitmap imageBitmap2 = (Bitmap)placaRecortada.Clone();
                Filtros.segmentar8conectado(imageBitmap2, placaRecortada, listaPini, listaPfim);

                Desenha(imageBitmapDest, listaPini, listaPfim, altura, largura, possiveisPlacas);

                //pictBoxImg.Image = placaRecortada;
                Bitmap imageBitmapR = (Bitmap)placaRecortada.Clone();
                Filtros.segmentar8conectado(imageBitmapR, placaRecortada, listaPini, listaPfim);

                altura = 0;
                largura = 0;
                List<Point> _listaPini = new List<Point>();
                List<Point> _listaPfim = new List<Point>();

                for (int i = 0; i < listaPini.Count; i++)
                {
                    altura = listaPfim[i].Y - listaPini[i].Y;
                    largura = listaPfim[i].X - listaPini[i].X;

                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        /*
                        na parte comentada do metodo encontra_placa la na parte de baixo tem que fazer um if pra ver se os pontos X e Y do retangulo verde está dentro das coordenadas da placa que foi achada tendeu ? Assim arruma os Retangulos verdes que foi achado fora da placa
                        larguraPlaca = imageRecortadaitmap.Width - areaPlaca.X - 1, areaPlaca.Width + 20;
                        alturaPlaca = imageRecortadaitmap.Height - areaPlaca.Y - 1, areaPlaca.Height + 10;
                        if (areaPlaca.X){
                        */

                        if (listaPini[i].X >= areaPlaca.X && listaPfim[i].X <= areaPlaca.X + areaPlaca.Width && listaPini[i].Y >= areaPlaca.Y && listaPfim[i].Y <= areaPlaca.Y + areaPlaca.Height)
                        {
                            // Desenha o retângulo verde na imagem (caractere detectado dentro da placa)
                            Filtros.desenhaRetangulo(imageBitmapDest, listaPini[i], listaPfim[i], Color.FromArgb(0, 255, 0));
                            _listaPini.Add(listaPini[i]);
                            _listaPfim.Add(listaPfim[i]);
                        }
                    }
                }

                placaRecortada = RecortarImagem(imageRecortadaitmap, placaComMargem);
                otsu = new Otsu();
                listaPini = new List<Point>();
                listaPfim = new List<Point>();
                // Aplica Otsu
                otsu.Convert2GrayScaleFast(placaRecortada);
                otsuThreshold = otsu.getOtsuThreshold((Bitmap)placaRecortada);
                otsu.threshold(placaRecortada, otsuThreshold);

                // Segmenta a imagem
                imageBitmap2 = (Bitmap)placaRecortada.Clone();
                Filtros.segmentar8conectado(imageBitmap2, placaRecortada, listaPini, listaPfim);
                for (int i = 0; i < listaPini.Count; i++)
                {
                    altura = listaPfim[i].Y - listaPini[i].Y;
                    largura = listaPfim[i].X - listaPini[i].X;

                    // acha a placa com base no tam dos caracteres
                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        Filtros.desenhaRetangulo(placaRecortada, listaPini[i], listaPfim[i], Color.FromArgb(0, 255, 0));
                        Filtros.reconheceDigito(placaRecortada, listaPini[i], listaPfim[i], cl_numeros, cl_letras);
                    }
                }

                pictBoxImg.Image = placaRecortada;
            }
            else
            {
                // Nenhuma placa foi encontrada
                Console.WriteLine("Nenhuma placa foi encontrada!");
            }

        }

        private static void Desenha(Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim, int altura, int largura, List<Rectangle> possiveisPlacas)
        {
            for (int i = 0; i < listaPini.Count; i++)
            {
                altura = listaPfim[i].Y - listaPini[i].Y;
                largura = listaPfim[i].X - listaPini[i].X;

                // acha a placa com base no tam dos caracteres
                if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                {
                    Rectangle caractere = new Rectangle(listaPini[i].X, listaPini[i].Y, largura, altura);
                    possiveisPlacas.Add(caractere);

                    // dsenha
                    Filtros.desenhaRetangulo(imageBitmapDest, listaPini[i], listaPfim[i], Color.FromArgb(0, 255, 0));
                }
            }
        }

        private static bool EhVerde(Color cor)
        {
            //
            if (cor.G > 100 && cor.G > cor.R + 20 && cor.G > cor.B + 20)
            {
                return true;
            }
            else if (cor.G > 80 && cor.G > cor.R + 10 && cor.G > cor.B + 10)
            {
                return true;
            }
            else if (cor.G > 60 && cor.G > cor.R && cor.G > cor.B)
            {
                return true;
            }

            return false;
        }

        private static (Rectangle, int) AgruparRetangulos(List<Rectangle> retangulos, Bitmap image)
        {
            if (retangulos.Count != 0)
            {
                int toleranciaEspaco = 500;
                int toleranciaAlinhamento = 500;

                var retangulosVerde = new List<Rectangle>();

                foreach (var retangulo in retangulos)
                {
                    Color cor = ObterCorDoRetangulo(retangulo, image);
                    //se vrd ele junta
                    if (EhVerde(cor))
                    {
                        retangulosVerde.Add(retangulo);
                    }
                }
                //se contagem de verdes for maior que 0 possivel placa
                if (retangulosVerde.Count > 0)
                {
                    //se tem pelo menos 3 ele tem uma placa (pode ser q recorte lugares errados)
                    retangulosVerde.Sort((r1, r2) => r1.X.CompareTo(r2.X));
                    Rectangle bloco = retangulosVerde[0];
                    int contagemCaracteres = 1;
                    //coonta caracteres e verifica a distancia de tolerancia e alinhamento
                    foreach (var retangulo in retangulosVerde.Skip(1))
                    {
                        if (Math.Abs(retangulo.X - bloco.X - bloco.Width) < toleranciaEspaco && Math.Abs(retangulo.Y - bloco.Y) < toleranciaAlinhamento)
                        {
                            bloco.Width = retangulo.X + retangulo.Width - bloco.X;
                            contagemCaracteres++;
                        }
                    }
                    //retorna area e contagem
                    return (bloco, contagemCaracteres);
                }
                Console.WriteLine("Numero de retangulos insuficiente! Placa não encontrada");
                return (Rectangle.Empty, 0);
            }
            Console.WriteLine("Numero de retangulos insuficiente! Placa não encontrada");
            return (Rectangle.Empty, 0);
        }

        private static Color ObterCorDoRetangulo(Rectangle retangulo, Bitmap image)
        {
            return image.GetPixel(retangulo.Left, retangulo.Top);
        }

        private static Bitmap RecortarImagem(Bitmap imagem, Rectangle area)
        {
            return imagem.Clone(area, imagem.PixelFormat);
        }

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

        public static Bitmap Erosao(Bitmap imageSrc, int[,] mascara, int tamX, int tamY, int[] origem)
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();
            int width = imageSrc.Width;
            int height = imageSrc.Height;

            for (int x = 1; x < width - tamX; x++)
            {
                for (int y = 1; y < height - tamY; y++)
                {
                    if (imageSrc.GetPixel(x, y).R > 128)
                    {

                        for (int i = 0; i < tamX; i++)
                        {
                            for (int j = 0; j < tamY; j++)
                            {
                                if (mascara[i, j] != 0)
                                {
                                    imageDest.SetPixel(x - origem[0] + j, y - origem[1] + i, Color.White);
                                }
                            }
                        }

                    }
                }
            }
            return imageDest;
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

    }
}