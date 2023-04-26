using System;
using System.Diagnostics;
using System.Security.Cryptography;


namespace MikroTik.Utils
{
    public class MikroTikConvert
    {
        public static string DateToMikroTikDate(DateTime date)
        {
            string[] months = {"", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            try
            {
                Debug.WriteLine("MONTH = {0}", date.Month);
                return String.Format("{0}/{1}/{2}", months[date.Month], date.Day, date.Year);
            }
            catch (Exception)
            {
            }

            return String.Empty;
        }
    }
}
