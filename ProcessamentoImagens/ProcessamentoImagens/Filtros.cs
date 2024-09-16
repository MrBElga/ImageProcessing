using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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




        //espelho vertical
        public static void verticalSemDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b, Raux, Baux, Gaux;
            for (int y = 0; y < height; y++)
            {
                int widthaux = width - 1;
                for (int x = 0; x < width; x++)
                {

                    //obtendo a cor do pixel

                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color coraux = imageBitmapSrc.GetPixel(widthaux, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    Raux = coraux.R;
                    Gaux = coraux.G;
                    Baux = coraux.B;

                    //nova cor
                    Color newcolor = Color.FromArgb(r, g, b);

                    imageBitmapDest.SetPixel(widthaux, y, newcolor);
                    newcolor = Color.FromArgb(Raux, Gaux, Baux);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                    widthaux--;

                }

            }
        }

        //espelho horizontal
        public static void horizontalSemDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b, Raux, Baux, Gaux;
            int heightaux = height - 1;
            for (int y = 0; y < height; y++)
            {

                for (int x = 0; x < width; x++)
                {

                    //obtendo a cor do pixel

                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color coraux = imageBitmapSrc.GetPixel(x, heightaux);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    Raux = coraux.R;
                    Gaux = coraux.G;
                    Baux = coraux.B;

                    //nova cor
                    Color newcolor = Color.FromArgb(r, g, b);

                    imageBitmapDest.SetPixel(x, heightaux, newcolor);
                    newcolor = Color.FromArgb(Raux, Gaux, Baux);
                    imageBitmapDest.SetPixel(x, y, newcolor);


                }
                heightaux--;
            }
        }



        public static void espelhoVeticalDMA(Bitmap imagebitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imagebitmapSrc.Width;
            int height = imagebitmapSrc.Height;
            int pixelSize = 3;

            // Lock dados bitmap origem
            BitmapData bitmapDataSrc = imagebitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            // Lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int srcStride = bitmapDataSrc.Stride;
            int dstStride = bitmapDataDst.Stride;

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                for (int y = 0; y < height; y++)
                {
                    byte* srcLine = src + (y * srcStride);
                    byte* dstLine = dst + (y * dstStride);

                    for (int x = 0; x < width; x++)
                    {
                        // Calcula a posição do pixel no lado direito da linha
                        int srcIndex = (width - x - 1) * pixelSize;

                        // Copia os valores dos pixels
                        dstLine[x * pixelSize] = srcLine[srcIndex];
                        dstLine[x * pixelSize + 1] = srcLine[srcIndex + 1];
                        dstLine[x * pixelSize + 2] = srcLine[srcIndex + 2];
                    }
                }
            }

            // Unlock imagem origem
            imagebitmapSrc.UnlockBits(bitmapDataSrc);
            // Unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }
        public static void ZhangSuen(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            // Lock bits for the source and destination bitmaps
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = bitmapDataSrc.Stride;
            int srcStride = bitmapDataSrc.Stride;
            int dstStride = bitmapDataDst.Stride;



            bool afinando = true;

            while (afinando)
            {
                afinando = false;
                List<PixelPoint> remPoints = new List<PixelPoint>();

                // First pass
                for (int j = 1; j < height - 1; j++)
                {
                    for (int i = 1; i < width - 1; i++)
                    {
                     
                        
                    }
                }

                foreach (var pixel in remPoints)
                {
                    setPixel(pixelsDest, stride, pixel.i, pixel.j, Color.White);
                }

                remPoints.Clear();

                // Second pass
                for (int j = 1; j < height - 1; j++)
                {
                    for (int i = 1; i < width - 1; i++)
                    {
                      
                    }
                }

                foreach (var pixel in remPoints)
                {
                    setPixel(pixelsDest, stride, pixel.i, pixel.j, Color.White);
                }

                if (remPoints.Count > 0)
                {
                    afinando = true;
                }

              
            }

            // Unlock the bits for both bitmaps
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            imageBitmapDest.UnlockBits(bitmapDataDest);
        }

        private static bool isPreto(byte[] pixels, int stride, int x, int y)
        {
            int index = y * stride + x * 3;
            return pixels[index] == 0 && pixels[index + 1] == 0 && pixels[index + 2] == 0;
        }

        private static bool isBranco(byte[] pixels, int stride, int x, int y)
        {
            int index = y * stride + x * 3;
            return pixels[index] == 255 && pixels[index + 1] == 255 && pixels[index + 2] == 255;
        }

        private static int calcConectividade(byte[] pixels, int stride, int x, int y)
        {
            int conectividade = 0;
            bool[] vetorPixel = new bool[8];
            carregaIntervaloP2P9(pixels, stride, vetorPixel, x, y);

            for (int k = 0; k < vetorPixel.Length - 1; k++)
            {
                if (!vetorPixel[k] && vetorPixel[k + 1])
                {
                    conectividade++;
                }
            }

            if (!vetorPixel[7] && vetorPixel[0])
            {
                conectividade++;
            }

            return conectividade;
        }

        private static int calcVizinho(byte[] pixels, int stride, int x, int y)
        {
            int vizinhos = 0;
            bool[] vetorPixel = new bool[8];
            carregaIntervaloP2P9(pixels, stride, vetorPixel, x, y);

            for (int k = 0; k < vetorPixel.Length; k++)
            {
                if (vetorPixel[k])
                {
                    vizinhos++;
                }
            }

            return vizinhos;
        }

        private static void carregaIntervaloP2P9(byte[] pixels, int stride, bool[] vetorPixelPoint, int x, int y)
        {
            vetorPixelPoint[0] = isPreto(pixels, stride, x, y - 1); // P2
            vetorPixelPoint[1] = isPreto(pixels, stride, x + 1, y - 1); // P3
            vetorPixelPoint[2] = isPreto(pixels, stride, x + 1, y); // P4
            vetorPixelPoint[3] = isPreto(pixels, stride, x + 1, y + 1); // P5
            vetorPixelPoint[4] = isPreto(pixels, stride, x, y + 1); // P6
            vetorPixelPoint[5] = isPreto(pixels, stride, x - 1, y + 1); // P7
            vetorPixelPoint[6] = isPreto(pixels, stride, x - 1, y); // P8
            vetorPixelPoint[7] = isPreto(pixels, stride, x - 1, y - 1); // P9
        }

        private static void setPixel(byte[] pixels, int stride, int x, int y, Color color)
        {
            int index = y * stride + x * 3;
            pixels[index] = color.B;
            pixels[index + 1] = color.G;
            pixels[index + 2] = color.R;
        }

        private static bool temBranco(byte[] pixels, int stride, int x, int y, int offsetX, int offsetY)
        {
            int newX = x + offsetX;
            int newY = y + offsetY;
            return isBranco(pixels, stride, newX, newY);
        }
    }

    struct PixelPoint
    {
        public int i, j;
    }
}
