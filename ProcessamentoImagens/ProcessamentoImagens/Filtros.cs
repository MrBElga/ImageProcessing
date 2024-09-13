using System;
using System.Drawing;
using System.Drawing.Imaging;
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


        //espelho com dma
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
                        r = *(src1++);
                        g = *(src1++);
                        b = *(src1++);
                        *(dst++) = (byte)b;
                        *(dst++) = (byte)g;
                        *(dst++) = (byte)r;
                    }
                }
            }

        }

        public static void ZhangSuen(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;

            // Lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            // Lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int strideSrc = bitmapDataSrc.Stride;
            int strideDst = bitmapDataDst.Stride;
            IntPtr scan0Src = bitmapDataSrc.Scan0;
            IntPtr scan0Dst = bitmapDataDst.Scan0;

            byte[] pixelsSrc = new byte[height * strideSrc];
            byte[] pixelsDst = new byte[height * strideDst];
            Marshal.Copy(scan0Src, pixelsSrc, 0, pixelsSrc.Length);
            Marshal.Copy(scan0Dst, pixelsDst, 0, pixelsDst.Length);

            bool afinando = true;
            while (afinando)
            {
                afinando = false;
                for (int i = 1; i < width - 1; i++)
                {
                    for (int j = 1; j < height - 1; j++)
                    {
                        int vizinhos = 0, conectividade = 0;

                        // P1 a P9
                        int p1 = GetPixel(pixelsSrc, strideSrc, i - 1, j - 1);
                        int p2 = GetPixel(pixelsSrc, strideSrc, i, j - 1);
                        int p3 = GetPixel(pixelsSrc, strideSrc, i + 1, j - 1);
                        int p4 = GetPixel(pixelsSrc, strideSrc, i + 1, j);
                        int p5 = GetPixel(pixelsSrc, strideSrc, i + 1, j + 1);
                        int p6 = GetPixel(pixelsSrc, strideSrc, i, j + 1);
                        int p7 = GetPixel(pixelsSrc, strideSrc, i - 1, j + 1);
                        int p8 = GetPixel(pixelsSrc, strideSrc, i - 1, j);
                        int p9 = GetPixel(pixelsSrc, strideSrc, i, j);

                        if (p9 == 0)
                        {
                            if (p2 >= 180) vizinhos++;
                            if (p3 >= 180) vizinhos++;
                            if (p4 >= 180) vizinhos++;
                            if (p5 >= 180) vizinhos++;
                            if (p6 >= 180) vizinhos++;
                            if (p7 >= 180) vizinhos++;
                            if (p8 >= 180) vizinhos++;

                            if (vizinhos >= 2 && vizinhos <= 6)
                            {
                                if ((p2 >= 180 && p3 == 0) || (p3 >= 180 && p4 == 0) ||
                                    (p4 >= 180 && p5 == 0) || (p5 >= 180 && p6 == 0) ||
                                    (p6 >= 180 && p7 == 0) || (p7 >= 180 && p8 == 0) ||
                                    (p8 >= 180 && p9 == 0) || (p9 >= 180 && p2 == 0))
                                {
                                    conectividade++;
                                }

                                if (conectividade == 1)
                                {
                                    SetPixel(pixelsDst, strideDst, i, j, 0);
                                    afinando = true;
                                }
                            }
                        }
                    }
                }
            }

            // Copia os dados para o bitmap de destino
            Marshal.Copy(pixelsDst, 0, scan0Dst, pixelsDst.Length);

            // Unlock imagem origem e destino
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        private static void SetPixel(byte[] pixels, int stride, int x, int y, int value)
        {
            int index = (y * stride) + (x * 3);
            pixels[index] = (byte)value; // B
            pixels[index + 1] = (byte)value; // G
            pixels[index + 2] = (byte)value; // R
        }

        private static int GetPixel(byte[] pixels, int stride, int x, int y)
        {
            // Verifica se (x, y) está fora dos limites da imagem
            if (x < 0 || x >= (stride / 3) || y < 0 || y >= (pixels.Length / stride))
            {
                return 255; // Branco (ou outro valor que indique fora dos limites)
            }
            else
            {
                int index = (y * stride) + (x * 3);
                return pixels[index]; // Retorna o valor do canal Blue
            }
         
        }
    }
}
