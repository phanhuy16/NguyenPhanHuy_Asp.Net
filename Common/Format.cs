using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Common
{
    public class Format
    {
        public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                str += "#";
            }
            if (str.Length > 0)
            {
                str = "." + str;
                string numFormat = string.Format("0:#,##0{0}", str);
                str = String.Format("{" + numFormat + "}", GT);
            }
            return str;
        }

        private static bool IsNumeric(object value)
        {
            return value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
        }
    }
}