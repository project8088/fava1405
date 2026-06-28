using System;
using System.Linq;

namespace Nikan.Common
{
    public static class ConvertDecToHex
    {

        public static string Convert(string decimalNumber)
        {

            try
            {
                var cr = ConvertToDecimal(long.Parse(decimalNumber));
                if (cr.Length == 7)
                    cr += "0";

                var hex = cr.ToArray();

                if (hex.Length == 8)
                {
                    var str1 = hex[1].ToString() + hex[0].ToString();
                    var str2 = hex[3].ToString() + hex[2].ToString();
                    var str3 = hex[5].ToString() + hex[4].ToString();
                    var str4 = hex[7].ToString() + hex[6].ToString();
                    return str1 + str2 + str3 + str4;
                }


            }
            catch (Exception er)
            {


            }

            return "";
        }

        public static string ConvertToDecimal(long number)

        {

            var hex2 = "";
            while (number > 0)
            {

                var remainder = number % 16; // 
                number = number / 16;

                hex2 += toHex(remainder);



            }
            return hex2;
        }
        static string toHex(long desc)
        {
            if (desc < 10)
                return desc.ToString();
            else if (desc == 10)
                return "A";
            else if (desc == 11)
                return "B";
            else if (desc == 12)
                return "C";
            else if (desc == 13)
                return "D";
            else if (desc == 14)
                return "E";
            else if (desc == 15)
                return "F";
            return "";

        }


    }
}
