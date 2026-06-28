using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Nikan.Common.Utilities
{
    public class DateTimeToPersianDateTimeConverter  
    {
        private readonly bool _includeHourMinute;
        private readonly string _separator;

        public DateTimeToPersianDateTimeConverter(string separator = "/", bool includeHourMinute = true)
        {
            _separator = separator;
            _includeHourMinute = includeHourMinute;
        } 
        public string toShamsiDateTime(DateTime info)
        {
            try
            {
                var year = info.Year;
                var month = info.Month;
                var day = info.Day;
                var persianCalendar = new PersianCalendar();
                var pYear = persianCalendar.GetYear(new DateTime(year, month, day, new GregorianCalendar()));
                var pMonth = persianCalendar.GetMonth(new DateTime(year, month, day, new GregorianCalendar()));
                var pDay = persianCalendar.GetDayOfMonth(new DateTime(year, month, day, new GregorianCalendar()));
                return _includeHourMinute ?
                    string.Format("{0}{1}{2}{1}{3} {4}:{5}", pYear, _separator, pMonth.ToString("00", CultureInfo.InvariantCulture), pDay.ToString("00", CultureInfo.InvariantCulture), info.Hour.ToString("00"), info.Minute.ToString("00"))
                    : string.Format("{0}{1}{2}{1}{3}", pYear, _separator, pMonth.ToString("00", CultureInfo.InvariantCulture), pDay.ToString("00", CultureInfo.InvariantCulture));


            }
            catch (Exception er)
            {

                
            }
            return "";
        
        
        }


        public DateTime? ShamsitoMiladi(string pdate)
        {
            try
            {
                PersianCalendar p = new PersianCalendar();
                string[] parts = pdate.Split('/', '-');
                var dta1 = p.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0, 0, 0, 0);
                return dta1;

            }
            catch (Exception er)
            {


            }

            return null;

        }






    }
}
