using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQL.Dapper
{
    class UseContrib
    {
        public void TestGet()
        {
            using (var connection = new SqlConnection(""))
            {
                var invoice = connection.Get<Invoice>(1);
            }
        }

        public void TestGetAll()
        {
            using (var connection = new SqlConnection(""))
            {
                var invoice = connection.GetAll<Invoice>(1);
            }
        }

        public void TestInsert()
        {
            using (var connection = new SqlConnection(""))
            {
                var key = connection.Insert(new Invoice { });

                var list = new List<Invoice>
                {
                    new Invoice{},
                    new Invoice{},
                    new Invoice{}
                };

                var insertedRows = connection.Insert(list);
            }
        }

        public void TestUpdate()
        {
            using (var connection = new SqlConnection(""))
            {
                var isSuccess = connection.Update(new Invoice { Id = 1, Code = "Update_Single_1" });

                var list = new List<Invoice>()
                {
                    new Invoice {Id = 1, Code = "Update_Many_1"},
                    new Invoice {Id = 2, Code = "Update_Many_2"},
                    new Invoice {Id = 3, Code = "Update_Many_3"}
                };

                isSuccess = connection.Update(list);
            }
        }

        public void TestDelete()
        {
            using (var connection = new SqlConnection(""))
            {
                var isSuccess = connection.Delete(new Invoice { Id = 1 });

                var list = new List<Invoice>()
                {
                    new Invoice {Id = 1},
                    new Invoice {Id = 2},
                    new Invoice {Id = 3}
                };

                isSuccess = connection.Delete(list);

                isSuccess = connection.DeleteAll<Invoice>();
            }
        }
    }
}
