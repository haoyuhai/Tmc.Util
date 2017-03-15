namespace Tmc.Util
{
    public static class DecimalExtension
    {
        /// <summary>
        /// 返回0.#格式的数字（即有多少位小数，就显示多少位），如果为null,则返回string.Empty
        /// </summary>
        public static string ToNormalizeString(this decimal? d)
        {
            if (d == null) return string.Empty;            
            return d.Value.ToString("0.##");
        }

        /// <summary>
        /// 返回0.#格式的数字（即有多少位小数，就显示多少位）
        /// </summary>
        public static string ToNormalizeString(this decimal d)
        {
            return d.ToString("0.##");
        }

        /// <summary>
        /// 格式化机票价格，四舍五入到十位
        /// </summary>
        public static decimal FormatPrice(this decimal ticketPrice)
        {
            return (int)(ticketPrice * 0.1m + 0.5m) * 10.0m;
        }
    }
}
