using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace ProcessamentoImagens
{
    class Filtros
    {
        //sem acesso direto a memoria
        public static void convert_to_gray(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //sem acesso direito a memoria
        public static void negativo(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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

                    //nova cor
                    Color newcolor = Color.FromArgb(255 - r, 255 - g, 255 - b);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //com acesso direto a memória
        public static void convert_to_grayDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;
            Int32 gs;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++); //está armazenado dessa forma: b g r 
                        g = *(src++);
                        r = *(src++);
                        gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                    }
                    src += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        //com acesso direito a memoria
        public static void negativoDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src1 = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src1++); //está armazenado dessa forma: b g r 
                        g = *(src1++);
                        r = *(src1++);

                        *(dst++) = (byte)(255 - b);
                        *(dst++) = (byte)(255 - g);
                        *(dst++) = (byte)(255 - r);
                    }
                    src1 += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }
        //1) Faça o espelho vertical e horizontal; 
        public static void espelhoVerticalSemDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDst)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                int aux = width -1;
                for (int x = 0; x < width; x++)
                {
               
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color corWidth = imageBitmapSrc.GetPixel(aux,y);

                    
                    //nova cor
                    imageBitmapDst.SetPixel(x, y, corWidth);
                    imageBitmapDst.SetPixel(aux, y, cor);
                    aux--;
                }
            }
        }

        public static void espelhoVerticalComDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDst)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            // Lock dados do bitmap origem e destino
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDst = imageBitmapDst.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int paddingSrc = bitmapDataSrc.Stride - (width * pixelSize);
            int paddingDst = bitmapDataDst.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    byte* srcRow = src + y * bitmapDataSrc.Stride; // Posição da linha na origem
                    byte* dstRow = dst + y * bitmapDataDst.Stride; // Posição da linha no destino

                    // Espelhar pixels de uma linha na vertical
                    for (int x = 0; x < width; x++)
                    {
                        byte* srcPixel = srcRow + x * pixelSize;               
                        byte* dstPixel = dstRow + (width - 1 - x) * pixelSize; 

                     
                        dstPixel[0] = srcPixel[0]; // Azul
                        dstPixel[1] = srcPixel[1]; // Verde
                        dstPixel[2] = srcPixel[2]; // Vermelho
                    }
                    // Adicionar padding (caso exista)
                    srcRow += paddingSrc;
                    dstRow += paddingDst;
                }
            }

            // Desbloquear os bits
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            imageBitmapDst.UnlockBits(bitmapDataDst);
        }


        public static void espelhoHorizontalSemDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDst)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height;y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obter a cor do pixel atual e do pixel espelhado na linha correspondente
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color corEspelhada = imageBitmapSrc.GetPixel(x,height - 1 - y);

                    // Trocar os pixels entre as posições (x, y) e (width - 1 - x, y)
                    imageBitmapDst.SetPixel(x, y, corEspelhada);
                    imageBitmapDst.SetPixel( x, height - 1 - y, cor);
                }

            }

        }
        public static void espelhoHorizontalComDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDst)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDst.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int paddingSrc = bitmapDataSrc.Stride - (width * pixelSize);
            int paddingDst = bitmapDataDst.Stride - (width * pixelSize);

            unsafe
            {
                byte* src1 = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst1 = (byte*)bitmapDataDst.Scan0.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    // O ponteiro `dst` é reposicionado para a linha espelhada correspondente
                    byte* dst = dst1 + (height - 1 - y) * bitmapDataDst.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        // Copiando pixels da origem para a linha correspondente da imagem de destino
                        dst[0] = src1[0]; // Azul
                        dst[1] = src1[1]; // Verde
                        dst[2] = src1[2]; // Vermelho

                        src1 += pixelSize;
                        dst += pixelSize;
                    }

                    src1 += paddingSrc; // Pulando o padding de origem, se houver
                }
            }

            // Unlock the bits
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            imageBitmapDst.UnlockBits(bitmapDataDst);
        }


        //2) Separe o Canal R(vermelho), o Canal G(verde) e o canal B(azul); 
        
        public static void separarCanais(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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

                    //nova cor
                    Color newcolor = Color.FromArgb(r, g, b);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void separarCanaisComDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();
                int b, g, r;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++); //está armazenado dessa forma: b g r 
                        g = *(src++);
                        r = *(src++);

                        *(dst++) = (byte)b;
                        *(dst++) = (byte)g;
                        *(dst++) = (byte)r;
                    }
                }
            }
        }

        //3) Tornar a Imagem Preto e Branco; 
        public static void pretoBranco(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtém a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    // Calcula o tom de cinza (média dos valores R, G, B)
                    int gray = (cor.R + cor.G + cor.B) / 3;

                    // Define preto ou branco dependendo do valor de cinza
                    Color newColor = gray < 128 ? Color.Black : Color.White;

                    imageBitmapDest.SetPixel(x, y, newColor);
                }
            }
        }


        //4) Faça a rotação 90º; 
        public static void rotacao90(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Gira 90 graus (sentido horário)
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    imageBitmapDest.SetPixel(height - y - 1, x, cor);
                }
            }
        }

        //5) Inverta os canais R(vermelho) com B(azul). 
        public static void inverterCanaisRBeB(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtém a cor original do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    // Troca os canais R e B
                    Color newColor = Color.FromArgb(cor.A, cor.B, cor.G, cor.R);

                    imageBitmapDest.SetPixel(x, y, newColor);
                }
            }
        }

        //6) Faça o espelho diagonal principal.Exemplo: 
        public static void espelhoDiagonalPrincipal(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Reflete em torno da diagonal principal
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    imageBitmapDest.SetPixel(y, x, cor);
                }
            }
        }

    }

}
