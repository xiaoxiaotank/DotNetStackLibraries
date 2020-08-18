using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Dapper.Transaction;

namespace SQL.Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Hello World!");
        }

		static void TestMethods()
        {
			var sqlOrderDetails = "SELECT TOP 5 * FROM OrderDetails";
			var sqlOrderDetail = "SELECT * FROM OrderDetails WHERE OrderDetailID = @OrderDetailID";
			var sqlCustomerInsert = "INSERT INTO Customers (CustomerName) VALUES(@CustomerName)";

			using (var connection = new SqlConnection(""))
			{
				var orderDetailList = connection.Query<OrderDetail>(sqlOrderDetails).ToList();
				var orderDetail = connection.QueryFirstOrDefault<OrderDetail>(sqlOrderDetail, new { OrderDetailID = 1 });
				var affectedRows = connection.Execute(sqlCustomerInsert, new { CustomerName = "张三" });
			}
		}

		static void TestParamter()
        {
			var sql = "";
			using (var connection = new SqlConnection(""))
			{
				// Anonymous
				var affectedRows = connection.Execute(sql, new { Kind = InvoiceKind.WebInvoice, Code = "Single_Insert_1" }, commandType: CommandType.StoredProcedure);

				// Dynamic
				var parameter = new DynamicParameters();
				parameter.Add("@Kind", InvoiceKind.WebInvoice, DbType.Int32, ParameterDirection.Input);
				parameter.Add("@Code", "Single_Insert_1", DbType.String, ParameterDirection.Input);
				parameter.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
				connection.Execute(sql, parameter, commandType: CommandType.StoredProcedure);
				var rowCount = parameter.Get<int>("@RowCount");

				// List
				connection.Query<Invoice>(sql, new { Kind = new[] { InvoiceKind.WebInvoice, InvoiceKind.StoreInvoice } }).ToList();

				// String
				connection.Query<Invoice>(sql, new { Code = new DbString { Value = "Single_Insert_1", IsFixedLength = false, Length = 9, IsAnsi = true } }).ToList();
			};
		}

		static void TestResult()
        {
			var sqlOrderDetails = "SELECT TOP 5 * FROM OrderDetails";

			using(var connection = new SqlConnection(""))
            {
				var anonymousList = connection.Query(sqlOrderDetails).ToList();
				var orderDetailList = connection.Query<OrderDetail>(sqlOrderDetails).ToList();
            }
		}

		static async Task TestUtilities()
        {
			var sql = "";
			using (var connection = new SqlConnection(""))
			{
				await connection.QueryAsync<Invoice>(sql);

				connection.Query<Invoice>(sql, buffered: false);

				using(var transaction = connection.BeginTransaction())
                {
					var affectedRows = connection.Execute(
						sql,
						new { Kind = InvoiceKind.WebInvoice, Code = "Single_Insert_1" },
						commandType: CommandType.StoredProcedure,
						transaction: transaction);

					// Dapper.Transaction
					var affectedRows2 = transaction.Execute(
						sql,
						new { Kind = InvoiceKind.WebInvoice, Code = "Single_Insert_1" },
						commandType: CommandType.StoredProcedure);


					transaction.Commit();
                }
			}
		}
	}

	public class Customer
	{
		public int CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string ContactName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
	}
	public class Order
	{
		public int OrderID { get; set; }
		public int CustomerID { get; set; }
		public int EmployeeID { get; set; }
		public DateTime OrderDate { get; set; }
		public int ShipperID { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }
	}

	public class OrderDetail
	{
		public int OrderDetailID { get; set; }
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public int Quantity { get; set; }
	}

	public class Invoice
    {
		// Dapper.Contrib 自增键，[ExplicitKey]为非自增键
		[Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public InvoiceDetail Detail { get; set; }

		// 该属性不可写
		[Write(false)]
        public bool NotExistProperty { get; set; }

		// 更新时不会更新该值
		[Computed]
        public DateTime Date { get; set; }
    }

	public class InvoiceDetail
    {

    }


	public class StoreInvoice : Invoice
    {

    }

	public class WebInvoice : Invoice
	{

    }

	public class InvoiceItem
    {

    }



	public enum InvoiceKind
    {
		WebInvoice,
		StoreInvoice
	}
}
