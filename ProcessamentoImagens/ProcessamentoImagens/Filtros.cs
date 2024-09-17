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
            int pixelSize = 3;  

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
                byte threshold = 228;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int srcIndex = (y * srcStride) + (x * pixelSize);
                        int dstIndex = (y * dstStride) + (x * pixelSize);

                        byte b = src[srcIndex];
                        byte g = src[srcIndex + 1];
                        byte r = src[srcIndex + 2];

                        int gray =((r + g + b) / pixelSize);

               
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
            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* dst = (byte*)bitmapDataDest.Scan0.ToPointer();

                bool afinando = true;
                while (afinando)
                {
                    afinando = false;

                    // Lista para armazenar os pontos a serem removidos
                    List<Ponto> remPoints = new List<Ponto>();

                    // Passo 1
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            if (preto(dst, x, y, stride))
                            {
                                int conect = calcConectividade(dst, x, y, stride);
                                if (conect == 1)
                                {
                                    int vizinhos = totVizinhos(dst, x, y, stride);
                                    if (vizinhos >= 2 && vizinhos <= 6)
                                    {
                                        if (branco(dst, x, y - 1, stride) || branco(dst, x + 1, y, stride) || branco(dst, x, y + 1, stride))
                                        {
                                            if (branco(dst, x + 1, y, stride) || branco(dst, x, y + 1, stride) || branco(dst, x - 1, y, stride))
                                                remPoints.Add(new Ponto { x = x, y = y });
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
                            dst[index] = 255;       // B
                            dst[index + 1] = 255;   // G
                            dst[index + 2] = 255;   // R
                        }
                        remPoints.Clear();
                        afinando = true;
                    }

                    // Passo 2
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            if (preto(dst, x, y, stride))
                            {
                                int conect = calcConectividade(dst, x, y, stride);
                                if (conect == 1)
                                {
                                    int vizinhos = totVizinhos(dst, x, y, stride);
                                    if (vizinhos >= 2 && vizinhos <= 6)
                                    {
                                        if (branco(dst, x, y - 1, stride) || branco(dst, x + 1, y, stride) || branco(dst, x - 1, y, stride))
                                        {
                                            if (branco(dst, x - 1, y, stride) || branco(dst, x, y + 1, stride) || branco(dst, x, y - 1, stride))
                                                remPoints.Add(new Ponto { x = x, y = y });
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
                            dst[index] = 255;       // B
                            dst[index + 1] = 255;   // G
                            dst[index + 2] = 255;   // R
                        }
                        remPoints.Clear();
                        afinando = true;
                    }
                }

                imageBitmapSrc.UnlockBits(bitmapDataSrc);
                imageBitmapDest.UnlockBits(bitmapDataDest);
            }
        }
        private unsafe static bool preto(byte* src, int x, int y, int stride)
        {
            int index = (y * stride) + (x * 3);
            byte b = src[index];
            byte g = src[index + 1];
            byte r = src[index + 2];
            return (r + g + b) / 3 == 0; 
        }

        private unsafe static bool branco(byte* src, int x, int y, int stride)
        {
            int index = (y * stride) + (x * 3);
            byte b = src[index];
            byte g = src[index + 1];
            byte r = src[index + 2];
            return (r + g + b) / 3 == 255; 
        }

        private unsafe static int calcConectividade(byte* src, int i, int j, int stride)
        {
            int conectividade = 0;
            int tamanho = 8;

            bool[] vetorPixel = new bool[tamanho];
            carregaMatriz(src, vetorPixel, i, j, stride);

            for (int k = 0; k < tamanho - 1; k++)
            {
                if (vetorPixel[k] && !vetorPixel[k + 1])
                {
                    conectividade++;
                }
            }
            if (vetorPixel[7] && !vetorPixel[0])
                conectividade++;

            return conectividade;
        }

        private unsafe static int totVizinhos(byte* src, int x, int y, int stride)
        {
            int vizinhos = 0;
            int tamanho = 8;

            bool[] vetorPixel = new bool[tamanho];
            carregaMatriz(src, vetorPixel, x, y, stride);

            for (int k = 0; k < tamanho; k++)
            {
                if (vetorPixel[k])
                {
                    vizinhos++;
                }
            }

            return vizinhos;
        }



        private unsafe static void carregaMatriz(byte* src, bool[] vetorDePonto, int x, int y, int stride)
        {
            vetorDePonto[0] = preto(src, x, y-1, stride);
            vetorDePonto[1] = preto(src, x + 1, y - 1, stride);
            vetorDePonto[2] = preto(src, x + 1, y , stride);
            vetorDePonto[3] = preto(src, x + 1, y + 1, stride);
            vetorDePonto[4] = preto(src, x, y + 1, stride);
            vetorDePonto[5] = preto(src, x - 1, y + 1, stride);
            vetorDePonto[6] = preto(src, x-1, y, stride);
            vetorDePonto[7] = preto(src, x - 1, y - 1, stride); 
        }


  
    }

    struct Ponto
    {
        public int x, y;
    }
}
