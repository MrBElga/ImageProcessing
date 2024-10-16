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

        public static Bitmap Dilatacao(Bitmap imageSrc, int[,] mascara, int tamX, int tamY, int[] origem )
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();
         
            int width = imageSrc.Width;
            int height = imageSrc.Height;
            //mascara do tamanho generico
            //faz a mascara
            int mat = mascara.Length;
            Console.WriteLine(mat);
            for (int x = tamX; x < width - tamX; x++)
            {
                for (int y = tamY; y < height - tamY; y++)
                {
                    int R = imageSrc.GetPixel(x, y).R;
                    if (R < 128)
                    {
                        //mascara
                        //0 eh o preto
                        //{1,1,1}
                        //{1,0,1}
                        //{1,1,1}
                        /*
                         *caso n generico
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
                        */
                        for (int j = 0; j < tamX; j++)
                        {
                            for (int i = 0; i < tamY; i++)
                            {
                                //mascara
                                //1 eh o preto
                                //{0,1,0}
                                //{1,1,1}
                                //{0,1,0}
                                if (mascara[i,j]!=0)
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

        public static Bitmap Erosao(Bitmap imageSrc, int[,] mascara, int tamX, int tamY, int[] origem)
        {
            Bitmap imageDest = (Bitmap)imageSrc.Clone();
            int width = imageSrc.Width;
            int height = imageSrc.Height;

            for (int x = origem[0]; x < width - (tamX - origem[0]); x++)
            {
                for (int y = origem[1]; y < height - (tamY - origem[1]); y++)
                {
                    if (imageSrc.GetPixel(x, y).R < 128)
                    {
                        bool flag = false;

                        //mascara
                        //1 eh o preto
                        //{0,1,0}
                        //{1,1,1}
                        //{0,1,0}
                        for (int i = 0; i < tamX && !flag; i++)
                        {
                            for (int j = 0; j < tamY && !flag; j++)
                            {
                                if (mascara[i, j] != 0 && !(i == origem[0] && j == origem[1]))
                                {
                                    //tem  vizinhos brancos
                                    if (imageSrc.GetPixel(x - origem[0] + i, y - origem[1] + j).R > 128)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                        }
                        if (flag)
                        {
                            imageDest.SetPixel(x, y, Color.White);
                        }
                    }
                }
            }
            return imageDest;
        }






    }
}
