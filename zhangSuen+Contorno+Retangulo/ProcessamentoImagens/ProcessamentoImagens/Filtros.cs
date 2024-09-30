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
                byte threshold = 221;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int srcIndex = (y * srcStride) + (x * pixelSize);
                        int dstIndex = (y * dstStride) + (x * pixelSize);

                        byte b = src[srcIndex];
                        byte g = src[srcIndex + 1];
                        byte r = src[srcIndex + 2];

                        int gray = (int)(0.299 * r + 0.587 * g + 0.114 * b);


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


        public static void borda(Bitmap imagemSrc, Bitmap imagemDst)
        {
            int width = imagemSrc.Width;
            int height = imagemSrc.Height;

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int menorX = x, maiorX = x, menorY = y, maiorY = y;
                    if (imagemSrc.GetPixel(x + 1, y).B == 0 && imagemSrc.GetPixel(x, y).B == 255)
                    {
                        // Inicializa os limites do retângulo
                   

                        imagemSrc.SetPixel(x, y, Color.Gray);
                        imagemDst.SetPixel(x, y, Color.Black);

                        bool flag;
                        do
                        {
                            flag = false;


                            if (imagemSrc.GetPixel(x + 1, y).B == 255 && imagemSrc.GetPixel(x + 1, y - 1).B == 0)
                            { // o da frente eh branco e o da diagonal eh preto
                                x++;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x + 1, y - 1).B == 255 && imagemSrc.GetPixel(x, y - 1).B == 0 && imagemSrc.GetPixel(x + 1, y).B != 0 )
                            { // diagonal eh branco e o de cima eh preto e o da frente n eh preto
                                x++;
                                y--;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x, y - 1).B == 255 && imagemSrc.GetPixel(x - 1, y - 1).B == 0 )
                            { //o de cima eh branco e o da diagonal eh preto 
                                y--;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x - 1, y - 1).B == 255 && imagemSrc.GetPixel(x - 1, y).B == 0 && imagemSrc.GetPixel(x, y - 1).B != 0)
                            { // diagonal eh branco e o de tras eh preto
                                x--;
                                y--;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x - 1, y).B == 255 && imagemSrc.GetPixel(x - 1, y + 1).B == 0)
                            { // o de tras eh branco e a diagonal de baixo eh preta
                                x--;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x - 1, y + 1).B == 255 && imagemSrc.GetPixel(x, y + 1).B == 0 && imagemSrc.GetPixel(x-1, y ).B != 0 )
                            {// o da diagonal de baixo eh branco e o de baixo eh preto
                                x--;
                                y++;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x, y + 1).B == 255 && imagemSrc.GetPixel(x + 1, y + 1).B == 0)
                            { // o de baixo eh branco e o da diagonal da direita de baixo eh preto
                                y++;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }
                            else if (imagemSrc.GetPixel(x + 1, y + 1).B == 255 && imagemSrc.GetPixel(x + 1, y).B == 0 && imagemSrc.GetPixel(x, y+1).B != 0)
                            { // o da diagonal da direita de baixo eh branco e o de cima eh 
                                x++;
                                y++;
                                imagemSrc.SetPixel(x, y, Color.Gray);
                                imagemDst.SetPixel(x, y, Color.Black);
                                flag = true;
                            }


                            // Atualiza os limites do retângulo
                            maiorX = Math.Max(maiorX, x);
                            menorX = Math.Min(menorX, x);
                            maiorY = Math.Max(maiorY, y);
                            menorY = Math.Min(menorY, y);

                        } while (flag);

                        // Desenha os retângulos em torno da região detectada
                        DesenharRetangulo(imagemDst, menorX, maiorX, menorY, maiorY);
                    }
                }
            }

        }


        private static void DesenharRetangulo(Bitmap imagemDst, int menorX, int maiorX, int menorY, int maiorY)
        {
            Color vermelho = Color.FromArgb(255, 1, 1);
            // Desenha a borda superior e inferior
            for (int i = menorX; i <= maiorX; i++)
            {
                imagemDst.SetPixel(i, menorY, vermelho);
                imagemDst.SetPixel(i, maiorY, vermelho);
            }

            // Desenha a borda esquerda e direita
            for (int i = menorY; i <= maiorY; i++)
            {
                imagemDst.SetPixel(menorX, i, vermelho);
                imagemDst.SetPixel(maiorX, i, vermelho);
            }
        }


    }

    struct Ponto
    {
        public int x, y;
    }
}
