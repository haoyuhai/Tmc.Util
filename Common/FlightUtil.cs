using System.Text.RegularExpressions;

namespace Tmc.Util
{
    public static class FlightUtil
    {
        /// <summary>
        /// 将0~100的折扣数据转换为一位小数的格式，如85转换为8.5折
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public static string GetDiscountString(int discount)
        {
            if (discount <= 0) return "";
            else if (discount == 100) return "全价";
            else if (discount > 100) return "";
            else return (discount / 10.0).ToString("0.0") + '折';
        }

        /// <summary>
        /// 票号是否有效，有效的票号格式为：\d{3}-\d{10}， 或者\d{13}
        /// </summary>
        public static bool IsTicketNoValid(string ticketNo)
        {
            string pattern1 = @"^\d{3}-\d{10}$";            
            Regex reg1 = new Regex(pattern1);
            
            return reg1.IsMatch(ticketNo);
        }
    }
}
