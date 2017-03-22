using System;
using System.Linq;

namespace Jaylan.Utilities
{
    public class GuidToNumStrHelper
    {
        public static string GenerateStringId()
        {
            var i = Guid.NewGuid().ToByteArray().Aggregate<byte, long>(1, (current, b) => current * ((int)b + 1));
            return $"{i - DateTime.Now.Ticks:x}";
        }


        public static long GenerateIntId()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }


        public static string UUId()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            var longGuid = BitConverter.ToInt64(buffer, 0);

            var value = System.Math.Abs(longGuid).ToString();

            var buf = new byte[value.Length];
            var p = 0;
            for (var i = 0; i < value.Length;)
            {
                var ph = System.Convert.ToByte(value[i]);

                var fix = 1;
                if ((i + 1) < value.Length)
                {
                    var pl = System.Convert.ToByte(value[i + 1]);
                    buf[p] = (byte) ((ph << 4) + pl);
                    fix = 2;
                }
                else
                {
                    buf[p] = (byte) (ph);
                }

                if ((i + 3) < value.Length)
                {
                    if (System.Convert.ToInt16(value.Substring(i, 3)) < 256)
                    {
                        buf[p] = System.Convert.ToByte(value.Substring(i, 3));
                        fix = 3;
                    }
                }
                p++;
                i = i + fix;
            }
            var buf2 = new byte[p];
            for (var i = 0; i < p; i++)
            {
                buf2[i] = buf[i];
            }
            var cRtn = Convert.ToBase64String(buf2);
            cRtn = cRtn.ToLower();
            cRtn = cRtn.Replace("/", "");
            cRtn = cRtn.Replace("+", "");
            cRtn = cRtn.Replace("=", "");
            if (cRtn.Length == 12)
            {
                return cRtn;
            }

            return UUId();
        }

    }
}
