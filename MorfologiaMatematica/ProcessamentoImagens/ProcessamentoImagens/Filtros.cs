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

        public static Bitmap Dilatacao(Bitmap imageSrc)
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();
         
            int width = imageSrc.Width;
            int height = imageSrc.Height;

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (imageSrc.GetPixel(x, y).R == 0)
                    {
                        //mascara
                        //0 eh o preto
                        //{1,1,1}
                        //{1,0,1}
                        //{1,1,1}
                        if(imageSrc.GetPixel(x , y).R == 0)
                        {
                            imageDest.SetPixel(x - 1, y, Color.Black);
                            imageDest.SetPixel(x + 1, y, Color.Black);
                            imageDest.SetPixel(x, y - 1, Color.Black);
                            imageDest.SetPixel(x, y + 1, Color.Black);
                            imageDest.SetPixel(x+1, y - 1, Color.Black);
                            imageDest.SetPixel(x + 1, y + 1, Color.Black); 
                            imageDest.SetPixel(x - 1, y - 1, Color.Black);
                            imageDest.SetPixel(x - 1, y + 1, Color.Black);
                        }

                    }
                }
            }

            return imageDest;
        }

        public static Bitmap Erosao(Bitmap imageSrc)
        {
            Bitmap imageDest = new Bitmap(imageSrc.Width, imageSrc.Height);
            int width = imageSrc.Width;
            int height = imageSrc.Height;

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    //mascara
                    //0 eh o preto
                    //{1,1,1}
                    //{1,0,1}
                    //{1,1,1}
                    if(imageSrc.GetPixel(x, y).R == 0)
                    {
                        if (imageSrc.GetPixel(x - 1, y).R == 255 || imageSrc.GetPixel(x + 1, y).R == 255 || imageSrc.GetPixel(x, y - 1).R == 255 || imageSrc.GetPixel(x, y + 1).R == 255)
                        {
                            imageDest.SetPixel(x, y, Color.White);
                        }
                        else
                        {
                            imageDest.SetPixel(x, y, Color.Black);
                        }
                    }
                    else
                    {
                        imageDest.SetPixel(x, y, Color.White);
                    }


                }
            }

            return imageDest;
        }




    }
}
