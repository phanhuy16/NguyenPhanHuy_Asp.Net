using NguyenPhanHuy_2122110062.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Common
{
    public class SettingHelper
    {
        private static ApplicationDbContext context = new ApplicationDbContext();

        public static string GetValue(string key)
        {
            var item = context.SystemSettings.SingleOrDefault(x => x.SettingKey == key);
            if (item != null)
            {
                return item.SettingValue;
            }

            return "";
        }
    }
}