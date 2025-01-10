using Dapper;
using NguyenPhanHuy_2122110062.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NguyenPhanHuy_2122110062.Common
{
    public class AccessStatistics
    {
        public static string StrConnect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public static StatisticalViewModel Statistical()
        {
            using (var connect = new SqlConnection(StrConnect))
            {
                var item = connect.QueryFirstOrDefault<StatisticalViewModel>("sp_Statistical", commandType: CommandType.StoredProcedure);
                return item;
            }
        }
    }
}