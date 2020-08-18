using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.Dapper
{
    class UseQuery
    {
        // dynamic
        public void QueryAnonymous()
        {
            var sql = "SELECT TOP 10 * FROM OrderDetails;";
            using (var conn = new SqlConnection(""))
            {
                // dynamic
                var orderDetail = conn.QueryFirstOrDefault(sql);  // conn.Query(sql).FirstOrDefault();

            }
        }

        public async Task QueryAnonymousAsync()
        {
            var sql = "SELECT TOP 10 * FROM OrderDetails;";
            using (var conn = new SqlConnection(""))
            {
                // dynamic
                var orderDetail = (await conn.QueryAsync(sql)).FirstOrDefault();
            }
        }

        // 强类型
        public void QueryStronglyTyped()
        {
            var sql = "SELECT TOP 10 * FROM OrderDetails;";
            using (var conn = new SqlConnection(""))
            {
                var orderDetails = conn.Query<OrderDetail>(sql);
            }
        }

        public async Task QueryStronglyTypedAsync()
        {
            var sql = "SELECT TOP 10 * FROM OrderDetails;";
            using (var conn = new SqlConnection(""))
            {
                var orderDetails = await conn.QueryAsync<OrderDetail>(sql);
            }
        }

        // 获取 1 : 1 关系的实体关系映射
        public void QueryMultiMappingOfOneToOne()
        {
            var sql = "SELECT * FROM Invoice AS A INNER JOIN InvoiceDetail AS B ON A.ID = B.InvoiceID;";
            using (var conn = new SqlConnection(""))
            {
                // Invoice : InvoiceDetail = 1 : 1
                var invoiceList = conn.Query<Invoice, InvoiceDetail, Invoice>(
                    sql,
                    (invoice, invoiceDetail) =>
                    {
                        invoice.Detail = invoiceDetail;
                        return invoice;
                    },
                    splitOn: "ID")
                .Distinct()
                .ToList();

            }
        }

        // 获取 1 : n 关系的实体关系映射
        public void QueryMultiMappingOfOneToMany()
        {
            var sql = "SELECT TOP 10 * FROM Orders AS A INNER JOIN OrderDetails AS B ON A.OrderID = B.OrderID;";
            using (var conn = new SqlConnection(""))
            {
                var orderDictionary = new Dictionary<int, Order>();
                // Order : OrderDetail = 1 : n
                var list = conn.Query<Order, OrderDetail, Order>(
                    sql,
                    (order, orderDetial) =>
                    {
                        if (!orderDictionary.TryGetValue(order.OrderID, out Order orderEntry))
                        {
                            orderEntry = order;
                            orderEntry.OrderDetails = new List<OrderDetail>();
                            orderDictionary[order.OrderID] = orderEntry;
                        }

                        orderEntry.OrderDetails.Add(orderDetial);
                        return orderEntry;
                    },
                    splitOn: "OrderID")
                .Distinct()
                .ToList();
            }
        }

        // 将同表数据转换为多种类型
        public void QueryMultiType()
        {
            var sql = "SELECT * FROM Invoice;";
            using(var conn = new SqlConnection(""))
            {
                var invoices = new List<Invoice>();
                using(var reader = conn.ExecuteReader(sql))
                {
                    var storeInvoiceParser = reader.GetRowParser<StoreInvoice>();
                    var webInvoiceParser = reader.GetRowParser<WebInvoice>();

                    while (reader.Read())
                    {
                        Invoice invoice = ((InvoiceKind)reader.GetInt32(reader.GetOrdinal("Kind"))) switch
                        {
                            InvoiceKind.StoreInvoice => storeInvoiceParser(reader),
                            InvoiceKind.WebInvoice => webInvoiceParser(reader),
                            _ => throw new Exception(),
                        };

                        invoices.Add(invoice);
                    }
                }
            }
        }


        // 执行多条查询语句
        public void QueryMultiple()
        {
            var sql = "SELECT * FROM Invoice WHERE InvoiceID = @InvoiceID; SELECT * FROM InvoiceItem WHERE InvoiceID = @InvoiceID;";
            using (var conn = new SqlConnection(""))
            {
                using(var multi = conn.QueryMultiple(sql, new { InvoiceID = 1 }))
                {
                    var invoice = multi.Read<Invoice>().First();
                    var invoiceItems = multi.Read<InvoiceItem>().ToList();
                }
            }
        }
    }
}
