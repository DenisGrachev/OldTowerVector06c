using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace vektorizer
{
    class Program
    {

        
        static void Main(string[] args)
        {
            int tilesInLine = 16;

            if (args.Length > 1)
            {
                tilesInLine = int.Parse(args[1]);
            }

            byte[] vektorTiles2 = new byte[tilesInLine * 8 * 4];


            byte[] vektorPal = new byte[16];

            Color[] palette = new Color[16];
            //Bitmap bitmap = new Bitmap("tiles.png");
            Bitmap bitmap = new Bitmap(args[0]);
            int width = bitmap.Width;
            int height = bitmap.Height;

            //one tile is 8 byte per bitplane
            int totalTiles = (height - 8) / 8 * (width / 8);
            byte[] vektorTiles = new byte[totalTiles * 8 * 4];

            //build pal from top 16 tiles
            for (int x=0;x<16;x++)
            {
                palette[x] = bitmap.GetPixel(x*8, 0);

                //convert to 8-bit color
                double red = Math.Round((double) ((palette[x].R * 8) / 256));
                double green = Math.Round((double)((palette[x].G * 8) / 256));
                double blue = Math.Round((double)((palette[x].B * 4) / 256));
                //add to vektor pal
                vektorPal[x] = (byte)(red + green*8 + blue*64);                
            }

            //byte bitbyte = 0;
            //Console.WriteLine(bitbyte | (1<<1));

            //save vektor palette
            File.WriteAllBytes(args[0]+".pal", vektorPal);

            //process tiles

            int tileNum = 0;
            for (int y = 8; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    
                    for (int yy = 0; yy < 8; yy++)
                    {                        
                        //ok now put dat color to tiles array %)
                        byte bitmap8=0, bitmap4=0, bitmap2=0, bitmap1=0;

                        for (int xx = 0; xx < 8; xx++)
                        {
                            //get pixel
                            Color pixel = bitmap.GetPixel(x + xx, y + yy);

                            //==============================================
                            //find color in palette
                            byte index = 0;
                            for (byte i = 0; i < 16; i++)
                            {
                                Color palColor = palette[i];
                                //if (palColor.R == pixel.R && palColor.G == pixel.G && palColor.B == pixel.B )
                                if (palColor == pixel)
                                {
                                    index = i;
                                    break;
                                }
                            }
                            //=============================================


                            //build a bytes for bitplanes
                            int shift = 7 - xx;
                            if (index != 0)
                            {
                                
                                if (  (index &(1<<3)) != 0)
                                {
                                    //which bit to turn on
                                    bitmap8 = (byte)(bitmap8 | (1 << shift));
                                }

                                if ((index & (1 << 2)) != 0)
                                {
                                    //which bit to turn on
                                    bitmap4 = (byte)(bitmap4 | (1 << shift));
                                }

                                if ((index & (1 << 1)) != 0)
                                {
                                    //which bit to turn on
                                    bitmap2 = (byte)(bitmap2 | (1 << shift));
                                }


                                if ((index & (1 << 0)) != 0)
                                {
                                    //which bit to turn on
                                    bitmap1 = (byte)(bitmap1 | (1 << shift));
                                }
                            }
                                                        
                        }

                        //vektorTiles[tileNum * 32 + yy + 0] = bitmap8;
                        //vektorTiles[tileNum * 32 + yy + 8] = bitmap4;
                        //vektorTiles[tileNum * 32 + yy +16] = bitmap2;
                        //vektorTiles[tileNum * 32 + yy +24] = bitmap1;                        

                        vektorTiles[tileNum * 32 + yy * 4 + 0] = bitmap8;
                        vektorTiles[tileNum * 32 + yy * 4 + 1] = bitmap4;
                        vektorTiles[tileNum * 32 + yy * 4 + 2] = bitmap2;
                        vektorTiles[tileNum * 32 + yy * 4 + 3] = bitmap1;

                    }


                    tileNum++;
                }

            }

            if (tilesInLine == 16)
            {
                File.WriteAllBytes(args[0] + ".tiles", vektorTiles);
            }
            else
            {
                Array.Copy(vektorTiles, vektorTiles2, vektorTiles2.Length);
                File.WriteAllBytes(args[0] + ".tiles", vektorTiles2);
            }


            //Console.ReadKey();
        }


    }
}
