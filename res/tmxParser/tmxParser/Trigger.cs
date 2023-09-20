using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmxParser
{
    public class Trigger
    {

        public enum TriggerType : byte
        {
            BASE = 0,
            PUT_ENEMY = 1,
            PUT_BLOCK = 2,
            PUT_FLOOR = 3,
            CLEAR = 4,
            EXIT = 5,
            BLOCK = 6
        }

        public byte id,x, y, range, type,range_type;

        public Trigger(byte id, string type, int x, int y,byte range)
        {
            this.id = id;

            this.x = (byte)(x / 8 - 1);
            this.y = (byte)(y / 8 - 1);
            this.range = range;

            switch (type)
            {
                case "base":
                    this.type = (byte)TriggerType.BASE;
                    break;
                case "put_enemy":
                    this.type = (byte)TriggerType.PUT_ENEMY;
                    break;
                case "put_block":
                    this.type = (byte)TriggerType.PUT_BLOCK;
                    break;
                case "put_floor":
                    this.type = (byte)TriggerType.PUT_FLOOR;
                    break;
                case "clear":
                    this.type = (byte)TriggerType.CLEAR;
                    break;
                case "exit":
                    this.type = (byte)TriggerType.EXIT;
                    break;
                case "block":
                    this.type = (byte)TriggerType.BLOCK;
                    break;
            }

            this.range_type = (byte)(this.type + 32 * this.range);

        }


    }
}
