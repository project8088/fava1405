using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.Common 
{
  public static class GlobalExtensions
    {


        public static int PercentToInt(this decimal input)
        {
            try
            {
                var c = input * 100;
                return Convert.ToInt32(c);
            }
            catch (Exception er)
            {

                 
            }

            return -1;



        }

        public static decimal IntToPercent(this int input)
        {
            try
            {
                var c = (decimal)input / 100;
                return c;
            }
            catch (Exception er)
            {


            }

            return -1;



        }

    }
}
