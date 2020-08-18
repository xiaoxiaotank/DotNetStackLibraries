using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SQL.Dapper
{
    /// <summary>
    /// 将数据存放至DataTable/DataSet
    /// </summary>
    class UseExecuteReader
    {
        public void Test()
        {
            using(var conn = new SqlConnection(""))
            {
                var reader = conn.ExecuteReader("SELECT * FROM Customers");
                var dt = new DataTable();
                dt.Load(reader);
            }
        }

        public async Task TestAsync()
        {
            using (var conn = new SqlConnection(""))
            {
                var reader = await conn.ExecuteReaderAsync("SELECT * FROM Customers");
                var dt = new DataTable();
                dt.Load(reader);
            }
        }
    }
}
