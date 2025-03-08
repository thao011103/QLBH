using System;
using System.Data;
using System.Data.SqlClient;

namespace BussinessLayer
{
        public static class Connection
        {
            // Thay chuỗi kết nối bằng thông tin của bạn
            private static string connectionString = @"Data Source=DII\THAO0111;Initial Catalog=QUANLY_BANHANG;Integrated Security=True";

            public static SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }
        }
}