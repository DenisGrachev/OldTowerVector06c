using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace tmxParser
{
    public class Program
    {
        public static void AppendAllBytes(string path, byte[] bytes)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void AppendByte(string path, byte byte_)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.WriteByte(byte_);
            }
        }


        static void Main(string[] args)
        {

            const int TILE_GUN_RIGHT = 52;
            const int TILE_GUN_LEFT = 54;

            const int TILE_EXIT = 44;
            const int TILE_DOT = 64;
            const int TILE_COIN = 65;
            const int TILE_HERO = 29-16;
            const int TILE_HERO_2 = 29-16-1;
            const int TILE_ENEMY_RIGHT = 30-16;
            const int TILE_ENEMY_LEFT = 31-16;

            //exits - up to 4 exits, if y=0 then skip
            //y
            //x
            //count
            //base tile

            byte[] exits = new byte[4*4];
            int exitsCount = 0;

            byte totalDots = 0;

            //our tilemap
            byte[] tileMap = new byte[50 * 14];

            byte[] heroes = new byte[4];

            //coins
            int coinsCount = 0;
            //reserved for 24 coins max
            // 2 bytes xy - 16bit
            // 2 bytes reserved for count and base tile
            byte[] coins = new byte[4 * 24];

            //enemies
            int enemiesCount = 0;
            //2bytes y,x
            //1byte direction
            //1byte count
            byte[] enemies = new byte[6 * 6];

            //yx - 2bytes
            //1 -byte direction 1-left 2-right
            //GUNS
            byte[] gunz = new byte[9 * 3];
            int gunsCount = 0;

            //collisions table
            byte[] collisions = new byte[256];

    
           // args[0] = "map00.tmx";
                  
            Console.WriteLine("TMX Parser by Denis Grachev");
            
            
            TmxMap map = new TmxMap(args[0]);

            Console.WriteLine("Build collision table");


            //for (int i=0;i< map.Tilesets[0].Tiles.Count;i++)
            //{
            //    Console.WriteLine(map.Tilesets[0].Tiles[i]);
            //}
            //TmxTilesetTile
            foreach (var tile in map.Tilesets[0].Tiles)
            {
                switch (tile.Value.Type)
                {

                    case "solid":
                        collisions[(tile.Key-16)*2+0] = 1;
                        collisions[(tile.Key-16)*2+1] = 1;
                        break;

                    case "solidv":
                        collisions[(tile.Key - 16) * 2 + 0] = 2;
                        collisions[(tile.Key - 16) * 2 + 1] = 2;
                        break;

                    case "solidh":
                        collisions[(tile.Key - 16) * 2 + 0] = 3;
                        collisions[(tile.Key - 16) * 2 + 1] = 3;
                        break;

                    default:
                        break;

                }
                //collisions[tile.Key,]
                //Console.WriteLine(tile.Value.Type);
                Console.WriteLine($"id: {tile.Key-16}  type: {tile.Value.Type}");
            }

       

            Console.WriteLine("===========================");


            Console.WriteLine("Parsing tilemap!");

            //int TILE_KEY = 105;            
            
            //import tiles from level layer
            for (byte j = 0; j < 50; j++)
            {
                for (byte i = 0; i < 14; i++)
                {
                    int tile = (map.TileLayers["level"].Tiles[i + j * 14].Gid);

                    //first 16 tiles it's  just a palette, so we skip they
                    //and skip zeros
                    if (tile > 17)
                    {
                        tileMap[i + j * 14] = (byte)((tile - 17) * 2);

                    }

                    
                    switch (tile-1)
                    {

                       

                        case TILE_COIN:
                            if (coinsCount < 24)
                            {
                               // Console.WriteLine("-COIN-");
                                coins[coinsCount * 4 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                coins[coinsCount * 4 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte

                                //base tile
                                coins[coinsCount * 4 + 2] = (byte)((tile - 16-1) * 2);

                                coinsCount++;
                                totalDots++;
                            }
                         break;

                        case TILE_DOT:
                            totalDots++;
                            break;

                        case TILE_EXIT:
                            if (exitsCount < 4)
                            {
             
                                exits[exitsCount * 4 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                exits[exitsCount * 4 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte

                                //base tile
                                exits[exitsCount * 4 + 2] = (byte)((tile - 16 - 1) * 2);

                                exitsCount++;
                            }
                            break;

                        case TILE_GUN_RIGHT:
                            if (gunsCount < 8)
                            {

                                gunz[gunsCount * 3 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                gunz[gunsCount * 3 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte

                                //base tile
                                gunz[gunsCount * 3 + 2] = 1;

                                gunsCount++;
                            }
                            break;

                        case TILE_GUN_LEFT:
                            if (gunsCount < 8)
                            {

                                gunz[gunsCount * 3 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                gunz[gunsCount * 3 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte

                                //base tile
                                gunz[gunsCount * 3 + 2] = 255;

                                gunsCount++;
                            }
                            break;


                    }
                    

                }
            }
           

            //build objects
            //import tiles from object layer
            for (byte j = 0; j < 50; j++)
            {
                for (byte i = 0; i < 14; i++)
                {
                    int tile = (map.TileLayers["objects"].Tiles[i + j * 14].Gid)-1;

                    switch (tile)
                        {
                        case TILE_HERO:                            
                            {
                                heroes[0] = j;
                                heroes[1] = (byte)(9+i);
                            }
                            break;

                        case TILE_HERO_2:
                            {
                                heroes[2] = j;
                                heroes[3] = (byte)(9 + i);
                            }
                            break;

                        case TILE_ENEMY_LEFT:
                            if (enemiesCount<6)
                            {
                                enemies[enemiesCount * 6 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                enemies[enemiesCount * 6 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte
                                enemies[enemiesCount * 6 + 2] = 1;//1-left 2-right
                                enemies[enemiesCount * 6 + 3] = 0;// (byte)(enemiesCount);
                                enemies[enemiesCount * 6 + 4] = (byte)(enemiesCount % 2);// (byte)enemiesCount;
                                enemies[enemiesCount * 6 + 5] = (byte)((9 + i) * 8);
                                enemiesCount++;
                            }
                            break;

                            case TILE_ENEMY_RIGHT:
                            if (enemiesCount < 6)
                            {
                                enemies[enemiesCount * 6 + 0] = j;//y// (byte)((j * 8) % 256);//low byte
                                enemies[enemiesCount * 6 + 1] = (byte)(9 + i);//x// (byte)((j * 8) / 256);//high byte
                                enemies[enemiesCount * 6 + 2] = 2;//1-left 2-right
                                enemies[enemiesCount * 6 + 3] = 0;// (byte)(enemiesCount);// (byte)enemiesCount;
                                enemies[enemiesCount * 6 + 4] = (byte)(enemiesCount % 2);// (byte)enemiesCount;
                                enemies[enemiesCount * 6 + 5] = (byte)((9 + i) * 8);
                                enemiesCount++;
                            }
                            break;


                       
                            default:
                            break;
                        }

                }
            }


            //File.WriteAllBytes(args[0] + ".coins", coins);
            //File.WriteAllBytes(args[0] + ".enemies", enemies);
            //File.WriteAllBytes(args[0] + ".mapa", tileMap);
           // File.WriteAllBytes(args[0] + ".doors", exits);

            File.WriteAllBytes(args[0] + ".mapa", coins);
            AppendAllBytes(args[0] + ".mapa", enemies);
            AppendAllBytes(args[0] + ".mapa", exits);            
            AppendAllBytes(args[0] + ".mapa", gunz);
            AppendByte(args[0] + ".mapa", totalDots);
            AppendAllBytes(args[0] + ".mapa", tileMap);
            AppendAllBytes(args[0] + ".mapa", heroes);


            File.WriteAllBytes("tiles.collisions", collisions);


            File.WriteAllBytes("gunz.gunz", gunz);

            Console.WriteLine();

           
            

            
            //===================================================

            //                Console.ReadKey();

        }
            

        }
}
