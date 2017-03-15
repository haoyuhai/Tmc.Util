using System; 

namespace Tmc.Util
{
    public static class Util
    {
        /// <summary>
        /// 指定的年，月是否为当前日期以后
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool IsFuture(int year, int month)
        {
            return new DateTime(year, month, 1) > DateTime.Today;
        }

        private static readonly Random random = new Random();

        public static int GetRandomInt(int length)
        {
            if (length == 1)
            {
                return random.Next(0, 10);
            }
            else if (length == 2)
            {
                return random.Next(10, 100);
            }
            else if (length == 3)
            {
                return random.Next(100, 1000);
            }
            else if (length == 4)
            {
                return random.Next(1000, 10000);
            }
            else if (length == 5)
            {
                return random.Next(10000, 100000);
            }
            else if (length == 6)
            {
                return random.Next(100000, 1000000);
            }
            else
            {
                throw new ArgumentOutOfRangeException("length");
            }
        }
    }
}
