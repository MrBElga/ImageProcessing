using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ProcessamentoImagens
{
    class Filtros
    {
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
