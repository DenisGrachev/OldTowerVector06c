using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmxParser
{
    public class Creature
    {

        public byte x, y, isPlayerControlled, type, tileNum, tilePage, HP, maxHP, level, id, statAttack, statDefence, statMagicAttack, statMagicDefence,MP;

        public int bigTileNum = 0;
        public byte[] data = new byte[32];
        enum CreatureType : byte
        {
            NONE = 0,
            KNIGHT = 1,
            MAGE = 2,
            BARBARIAN = 3,
            DRUID = 4,
            SKELETON = 5,
            ENT = 6,
            BERSERKER = 7,
            PHOENIX = 8,
            WIZARD = 9,
            WARLOCK = 10
        }

        public Creature(int id,string heroType,int x,int y,int level)
        {
            this.tilePage = 16 + 4;
            this.id = (byte)id;
            this.level = (byte)level;
            this.x = (byte)(x/8-1);
            this.y = (byte)(y/8-1);

            switch (heroType)
            {
                case "knight":
                    this.isPlayerControlled = 1;
                    this.tileNum = (byte)(320-256);
                    this.type = (byte)CreatureType.KNIGHT;
                    this.HP = (byte)(140 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 21);
                    this.statDefence = (byte)(level + 8);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10);
                    this.MP = 0;
                    break;

                case "mage":
                    this.isPlayerControlled = 1;
                    this.tileNum = (byte)(328 - 256);
                    this.type = (byte)CreatureType.MAGE;
                    this.HP = (byte)(130 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 17);
                    this.statDefence = (byte)(level + 4);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10);
                    this.MP = (byte)(100+level*5);
                    break;

                case "barbarian":
                    this.isPlayerControlled = 1;
                    this.tileNum = (byte)(336 - 256);
                    this.type = (byte)CreatureType.BARBARIAN;
                    this.HP = (byte)(150 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 24);
                    this.statDefence = (byte)(level + 6);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10);
                    this.MP = 0;
                    break;

                case "druid":
                    this.isPlayerControlled = 1;
                    this.tileNum = (byte)(344 - 256);
                    this.type = (byte)CreatureType.DRUID;
                    this.HP = (byte)(135 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 18);
                    this.statDefence = (byte)(level + 5);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10);
                    this.MP = (byte)(90 + level * 5);
                    break;

                case "wizard":
                    this.isPlayerControlled = 1;
                    this.tileNum = (byte)(384 - 256);
                    this.type = (byte)CreatureType.WIZARD;
                    if (level > 64)
                    {
                        level -= 64;
                        this.HP = (byte)(130 + 5 * level);
                        this.maxHP = HP;
                        this.statAttack = (byte)(level + 17);
                        this.statDefence = (byte)(level + 4);
                        this.statMagicAttack = (byte)(level + 12);
                        this.statMagicDefence = (byte)(level + 12);
                        this.MP = (byte)(100 + level * 5);
                        level += 64;
                    }
                    else
                    {
                        this.HP = (byte)(130 + 5 * level);
                        this.maxHP = HP;
                        this.statAttack = (byte)(level + 17);
                        this.statDefence = (byte)(level + 4);
                        this.statMagicAttack = (byte)(level + 12);
                        this.statMagicDefence = (byte)(level + 12);
                        this.MP = (byte)(100 + level * 5);
                    }
                    break;

                case "skeleton":
                    this.isPlayerControlled = 0;
                    this.tileNum = (byte)(264 - 256);
                    this.type = (byte)CreatureType.SKELETON;
                    this.HP = (byte)(60 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 10);
                    this.statDefence = (byte)(level + 3);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10);
                    break;

                case "ent":
                    this.isPlayerControlled = 0;
                    this.tileNum = (byte)(272 - 256);
                    this.type = (byte)CreatureType.ENT;
                    this.HP = (byte)(65 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 14);
                    this.statDefence = (byte)(level + 4);
                    this.statMagicAttack = 0;// 0 - immunity to magic (byte)(level + 10);
                    this.statMagicDefence =  (byte)(level + 10);
                    break;

                case "berserker":
                    this.isPlayerControlled = 0;
                    this.tileNum = (byte)(256 - 256);
                    this.type = (byte)CreatureType.BERSERKER;
                    this.HP = (byte)(80 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 18);
                    this.statDefence = (byte)(level + 5);
                    this.statMagicAttack = (byte)(level + 10);
                    this.statMagicDefence = 0;//immunity for physical attacks (byte)(level + 10);
                    break;

                case "phoenix":
                    this.isPlayerControlled = 0;
                    this.tileNum = (byte)(280 - 256);
                    this.type = (byte)CreatureType.PHOENIX;
                    this.HP = (byte)(70 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 16);
                    this.statDefence = (byte)(level + 4);
                    this.statMagicAttack = 0;//immunity for magic attacks(byte)(level + 10);
                    this.statMagicDefence = 0;//immunity for physical attacks (byte)(level + 10);
                    break;

                case "warlock":
                    this.isPlayerControlled = 0;
                    this.tileNum = (byte)(392 - 256);
                    this.type = (byte)CreatureType.WARLOCK;
                    this.HP = (byte)(90 + 5 * level);
                    this.maxHP = HP;
                    this.statAttack = (byte)(level + 17);
                    this.statDefence = (byte)(level + 5);
                    this.statMagicAttack = (byte)(level + 10); // 0;//immunity for magic attacks(byte)(level + 10);
                    this.statMagicDefence = (byte)(level + 10); //0;//immunity for physical attacks (byte)(level + 10);
                    break;


            }
        }

    }
}
