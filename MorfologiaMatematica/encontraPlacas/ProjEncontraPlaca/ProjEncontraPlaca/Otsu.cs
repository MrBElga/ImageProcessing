using System.Drawing;
using System.Drawing.Imaging;

namespace ProjEncontraPlaca
{
    internal class Otsu
    {
        //função é usada para calcular os valores q na equação
        private float Px(int ini, int fim, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = ini; i <= fim; i++)
                sum = sum + hist[i];

            return (float)sum;
        }

        //função é usada para calcular os valores médios na equação (mu)
        private float Mx(int ini, int fim, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = ini; i <= fim; i++)
                sum = sum + i * hist[i];

            return (float)sum;
        }

        //busca a posição do maior elemento no vetor
        private int findMax(float[] vec, int n)
        {
            float maior = 0;
            int pos = 0;
            int i;

            for (i = 1; i < n - 1; i++)
            {
                if (vec[i] > maior)
                {
                    maior = vec[i];
                    pos = i;
                }
            }
            return pos;
        }

        //calcula o histograma da imagem
        private unsafe void getHistogram(byte* p, int width, int height, int stride, int[] hist)
        {
            hist.Initialize();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * 3; j += 3)
                {
                    hist[p[i * stride + j]]++;
                }
            }
        }

        //encontra threshold OTSU
        public int getOtsuThreshold(Bitmap bmp)
        {
            byte t = 0;
            float[] vet = new float[256];
            int[] hist = new int[256];
            vet.Initialize();

            float p1, p2, p12;
            int k;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();

                getHistogram(p, bmp.Width, bmp.Height, bmData.Stride, hist);

                //percorrer todos os valores t possíveis e maximiza a variação entre as classes
                for (k = 1; k < 255; k++)
                {
                    p1 = Px(0, k, hist);
                    p2 = Px(k + 1, 255, hist);
                    p12 = p1 * p2;
                    if (p12 == 0)
                        p12 = 1;
                    float diff = (Mx(0, k, hist) * p2) - (Mx(k + 1, 255, hist) * p1);
                    vet[k] = (float)diff * diff / p12;
                }
            }
            bmp.UnlockBits(bmData);

            t = (byte)findMax(vet, 256);

            return t;
        }

        public void Convert2GrayScaleFast(Bitmap bmp)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();
                int stopAddress = (int)p + bmData.Stride * bmData.Height;
                while ((int)p != stopAddress)
                {
                    p[0] = (byte)(.299 * p[2] + .587 * p[1] + .114 * p[0]);
                    p[1] = p[0];
                    p[2] = p[0];
                    p += 3;
                }
            }
            bmp.UnlockBits(bmData);
        }

        public void threshold(Bitmap bmp, int thresh)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();
                int h = bmp.Height;
                int w = bmp.Width;
                int ws = bmData.Stride;

                for (int i = 0; i < h; i++)
                {
                    byte* row = &p[i * ws];
                    for (int j = 0; j < w * 3; j += 3)
                    {
                        row[j] = (byte)((row[j] > (byte)thresh) ? 255 : 0);
                        row[j + 1] = (byte)((row[j + 1] > (byte)thresh) ? 255 : 0);
                        row[j + 2] = (byte)((row[j + 2] > (byte)thresh) ? 255 : 0);
                    }
                }
            }
            bmp.UnlockBits(bmData);
        }
    }
}