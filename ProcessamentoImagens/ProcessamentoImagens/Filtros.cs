using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

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

        public static void pretoeBranco(Bitmap sourceBitmap, Bitmap imageBitmapDest)
        {
            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            int pixelSize = 3;  // 3 bytes per pixel for 24bpp RGB

            // Lock bits for the source and destination bitmaps
            BitmapData bitmapDataSrc = sourceBitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int srcStride = bitmapDataSrc.Stride;
            int dstStride = bitmapDataDest.Stride;

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDest.Scan0.ToPointer();
                byte threshold = 240;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int srcIndex = (y * srcStride) + (x * pixelSize);
                        int dstIndex = (y * dstStride) + (x * pixelSize);

                        byte b = src[srcIndex];
                        byte g = src[srcIndex + 1];
                        byte r = src[srcIndex + 2];

                        byte gray = (byte)((r + g + b) / pixelSize);

               
                        if (gray >= threshold)
                        {
                            dst[dstIndex] = 255;         
                            dst[dstIndex + 1] = 255;    
                            dst[dstIndex + 2] = 255;     
                        }
                        else
                        {
                            dst[dstIndex] = 0;           
                            dst[dstIndex + 1] = 0;       
                            dst[dstIndex + 2] = 0;       
                        }
                    }
                }
            }

            // Unlock the bits for both bitmaps
            sourceBitmap.UnlockBits(bitmapDataSrc);
            imageBitmapDest.UnlockBits(bitmapDataDest);
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
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bitmapDataSrc.Stride;
            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDest.Scan0.ToPointer();

                bool afinando = true;
                while (afinando)
                {
                    afinando = false;

                    // Lista para armazenar os pontos a serem removidos
                    List<PixelPoint> remPoints = new List<PixelPoint>();

                    // Passo 1
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            if (preto(src, x, y, stride))
                            {
                                int conect = calcConectividade(src, x, y, stride);
                                if (conect == 1)
                                {
                                    int vizinhos = totVizinhos(src, x, y, stride);
                                    if (vizinhos >= 2 && vizinhos <= 6)
                                    {
                                        if (temBranco(src, x, y, stride))
                                        {
                                            remPoints.Add(new PixelPoint { x = x, y = y });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (remPoints.Count > 0)
                    {
                        foreach (var pixel in remPoints)
                        {
                            int index = (pixel.y * stride) + (pixel.x * pixelSize);
                            dst[index] = 255;       
                            dst[index + 1] = 255;   
                            dst[index + 2] = 255;   
                        }
                        remPoints.Clear();
                     

                    }

                    // Passo 2
                    remPoints.Clear();

                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            if (preto(src, x, y, stride))
                            {
                                int conect = calcConectividade(src, x, y, stride);
                                if (conect == 1)
                                {
                                    int vizinhos = totVizinhos(src, x, y, stride);
                                    if (vizinhos >= 2 && vizinhos <= 6)
                                    {
                                        if (temBranco(src, x, y, stride))
                                        {
                                            remPoints.Add(new PixelPoint { x = x, y = y });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (remPoints.Count > 0)
                    {
                        foreach (var pixel in remPoints)
                        {
                            int index = (pixel.y * stride) + (pixel.x * pixelSize);
                            dst[index] = 255;       
                            dst[index + 1] = 255;   
                            dst[index + 2] = 255;   
                        }
                        remPoints.Clear();
                        afinando = true; 
                    }

                    src = dst;
                }

           
                imageBitmapSrc.UnlockBits(bitmapDataSrc);
                imageBitmapDest.UnlockBits(bitmapDataDest);
            }
        }

        private unsafe static bool preto(byte* src, int x, int y, int stride)
        {
            // int index = (y * stride) + (x * 3);
            int index = (y * stride) + (x * 3);
            byte b = src[index];
            byte g = src[index + 1];
            byte r = src[index + 2];


            byte gray = (byte)((r + g + b) / 3);


            if (gray >= 240)
            {
                return false;
            }
            else
            {
                return true;
            }
   
        }

        private unsafe static bool branco(byte* src, int x, int y, int stride)
        {
            // int index = (y * stride) + (x * 3);
            int index = (y * stride) + (x * 3);
            byte b = src[index];
            byte g = src[index + 1];
            byte r = src[index + 2];


            byte gray = (byte)((r + g + b) / 3);


            if (gray >= 240)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    
    
        private unsafe static int calcConectividade(byte* src, int i, int j, int stride)
        {
            int conectividade = 0;
            int tamanho = 8;

            bool[] vetorPixel = new bool[tamanho];
            carregaIntervaloP2P9(src, vetorPixel, i, j, stride);

            for (int k = 0; k < tamanho - 1; k++)
            {
                if (vetorPixel[k] && !vetorPixel[k + 1])
                {
                    conectividade++;
                }
            }

            if (vetorPixel[tamanho - 1] && !vetorPixel[0])
            {
                conectividade++;
            }

            return conectividade;
        }

        private unsafe static int totVizinhos(byte* src, int i, int j, int stride)
        {
            int vizinhos = 0;
            int tamanho = 8;

            bool[] vetorPixel = new bool[tamanho];
            carregaIntervaloP2P9(src, vetorPixel, i, j, stride);

            for (int k = 0; k < tamanho; k++)
            {
                if (!(i == k && j == k) &&!vetorPixel[k]) // Conta os pixels pretos
                {
                    vizinhos++;
                }
            }

            return vizinhos;
        }



        private unsafe static void carregaIntervaloP2P9(byte* src, bool[] vetorPixelPoint, int i, int j, int stride)
        {
            vetorPixelPoint[0] = branco(src, i, j - 1, stride);   // P2
            vetorPixelPoint[1] = branco(src, i + 1, j - 1, stride); // P3
            vetorPixelPoint[2] = branco(src, i + 1, j, stride);     // P4
            vetorPixelPoint[3] = branco(src, i + 1, j + 1, stride); // P5
            vetorPixelPoint[4] = branco(src, i, j + 1, stride);     // P6
            vetorPixelPoint[5] = branco(src, i - 1, j + 1, stride); // P7
            vetorPixelPoint[6] = branco(src, i - 1, j, stride);     // P8
            vetorPixelPoint[7] = branco(src, i - 1, j - 1, stride); // P9
        }


        private unsafe static bool temBranco(byte* src, int x, int y, int stride)
        {
                                           
            if (branco(src,  x,  y-1,  stride) || branco(src, x+1, y, stride) || branco(src, x-1, y, stride) && branco(src, x + 1, y, stride) || branco(src, x , y+1, stride) || branco(src, x, y-1, stride))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    struct PixelPoint
    {
        public int x, y;
    }
}
