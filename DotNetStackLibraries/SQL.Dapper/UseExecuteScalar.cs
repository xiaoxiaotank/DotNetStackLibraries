using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SQL.Dapper
{
    /// <summary>
    /// 读取第一行第一列
    /// </summary>
    class UseExecuteScalar
    {
        public void Test()
        {
            using(var conn = new SqlConnection(""))
            {
                var name = conn.ExecuteScalar<string>("SELECT Name FROM Customers WHERE CustomerId = 1;");
            }
        }

        public async Task TestAsync()
        {
            using (var conn = new SqlConnection(""))
            {
                var name = await conn.ExecuteScalarAsync<string>("SELECT Name FROM Customers WHERE CustomerId = 1;");
            }
        }
    }
}
