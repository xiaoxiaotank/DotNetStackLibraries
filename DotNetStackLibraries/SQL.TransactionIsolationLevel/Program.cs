using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace SQL.TransactionIsolationLevel
{
    class Program
    {
        static void Main(string[] args)
        {
            new MyDbContext().Database.Migrate();
            TestReadUncommitted();
            //TestSerializable();
            Console.ReadKey();
        }
        #region 事务先进行Insert操作，其他任务进行更新操作。Insert时会在事务执行期间添加行锁，update时未使用索引的情况下进行Where会进行全表扫描，从而需要扫描刚insert的数据行，由于有行锁，所以会阻塞

        static void TestReadUncommitted()
        {
            var name = DateTime.Now.ToString();
            var hasInsertA = false;

            var task1 = Task.Run(() =>
            {
                var ctx = new MyDbContext();
                using (var ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }))
                {
                    var a = new A()
                    {
                        Name = name
                    };
                    var b = new B()
                    {
                        Name = name
                    };
                    ctx.As.Add(a);
                    ctx.SaveChanges();
                    Console.WriteLine("Transaction：Insert A complete");
                    hasInsertA = true;

                    Task.Delay(TimeSpan.FromSeconds(10)).Wait();
                    ctx.Bs.Add(b);
                    ctx.SaveChanges();
                    ts.Complete();
                }
                Console.WriteLine("Transaction: Task 1 complete");
            });

            #region 在产生交集的情况下，事务会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE [As]
            //        SET Name = {0},
            //            HasUpdated = 1
            //    ", name);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //}); 
            #endregion

            #region 在产生交集的情况下，事务会导致阻塞
            //var task2 = Task.Run(() =>
            //    {
            //        while (!hasInsertA) ;

            //        var ctx = new MyDbContext();
            //        Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //        ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE [As]
            //        SET Name = {0},
            //            HasUpdated = 1
            //        WHERE Id = 17
            //    ", name);
            //        Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //    }); 
            #endregion

            #region 在未产生交集、未使用索引（即全表扫描）的情况下，由于需要扫描上方事务中的数据（实际上产生了交集），所以会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE A
            //        SET Name = {0}
            //        FROM [As] A WITH(INDEX(0))
            //        WHERE HasUpdated = 1
            //    ", name);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //});
            #endregion

            #region 在未产生交集、强制使用索引的情况下，不会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE A
            //        SET Name = {0}
            //        FROM [As] A WITH(INDEX(IX_As_HasUpdated))
            //        WHERE HasUpdated = 1 
            //    ", name);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //});
            #endregion

            #region  在未产生交集的情况下，使用主键进行过滤（使用了索引）不会导致阻塞
            var task2 = Task.Run(() =>
            {
                while (!hasInsertA) ;

                var ctx = new MyDbContext();
                Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
                var a = ctx.As.First(a => a.HasUpdated);
                Console.WriteLine($"Task 2 has get updating entity: {DateTime.Now}");
                ctx.Database.ExecuteSqlRaw(@" 
                    UPDATE [As]
                    SET Name = {0}
                    WHERE Id = {1}
                ", name, a.Id);
                Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            });
            #endregion

            Task.WhenAll(task1, task2).Wait();
            Console.WriteLine("OK");
        }

        static void TestSerializable()
        {
            var name = DateTime.Now.ToString();
            var hasInsertA = false;

            var task1 = Task.Run(() =>
            {
                var ctx = new MyDbContext();
                using (var ts = new TransactionScope())
                {
                    var a = new A()
                    {
                        Name = name
                    };
                    var b = new B()
                    {
                        Name = name
                    };
                    ctx.As.Add(a);
                    ctx.SaveChanges();
                    Console.WriteLine("Transaction：Insert A complete");
                    hasInsertA = true;

                    Task.Delay(TimeSpan.FromSeconds(10)).Wait();
                    ctx.Bs.Add(b);
                    ctx.SaveChanges();
                    ts.Complete();
                }
                Console.WriteLine("Transaction: Task 1 complete");
            });

            #region 在产生交集的情况下，事务会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE [As]
            //        SET Name = {0},
            //            HasUpdated = 1
            //    ", name);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //}); 
            #endregion

            #region 在产生交集的情况下，事务会导致阻塞
            //var task2 = Task.Run(() =>
            //    {
            //        while (!hasInsertA) ;

            //        var ctx = new MyDbContext();
            //        Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //        ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE [As]
            //        SET Name = {0},
            //            HasUpdated = 1
            //        WHERE Id = 17
            //    ", name);
            //        Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //    }); 
            #endregion

            #region 在未产生交集、未使用索引（即全表扫描）的情况下，由于需要扫描上方事务中的数据（实际上产生了交集），所以会导致阻塞
            var task2 = Task.Run(() =>
            {
                while (!hasInsertA) ;

                var ctx = new MyDbContext();
                Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
                ctx.Database.ExecuteSqlRaw(@" 
                    UPDATE A
                    SET Name = {0}
                    FROM [As] A WITH(INDEX(0))
                    WHERE HasUpdated = 1
                ", name);
                Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            });
            #endregion

            #region 在未产生交集、强制使用索引的情况下，不会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE A
            //        SET Name = {0}
            //        FROM [As] A WITH(INDEX(IX_As_HasUpdated))
            //        WHERE HasUpdated = 1 
            //    ", name);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //});
            #endregion

            #region  在未产生交集的情况下，使用主键进行过滤（使用了索引）不会导致阻塞
            //var task2 = Task.Run(() =>
            //{
            //    while (!hasInsertA) ;

            //    var ctx = new MyDbContext();
            //    Console.WriteLine($"Task 2 ready to start: {DateTime.Now}");
            //    var a = ctx.As.First(a => a.HasUpdated);
            //    Console.WriteLine($"Task 2 has get updating entity: {DateTime.Now}");
            //    ctx.Database.ExecuteSqlRaw(@" 
            //        UPDATE [As]
            //        SET Name = {0}
            //        WHERE Id = {1}
            //    ", name, a.Id);
            //    Console.WriteLine($"Task 2 complete: {DateTime.Now}");
            //});
            #endregion

            Task.WhenAll(task1, task2).Wait();
            Console.WriteLine("OK");
        } 
        #endregion
    }
}
