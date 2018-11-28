namespace AspNet.WebApi.ConnectDbByEF.MySql.Migrations
{
    using AspNet.WebApi.ConnectDbByEF.MySql.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNet.WebApi.ConnectDbByEF.MySql.Models.DbModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AspNet.WebApi.ConnectDbByEF.MySql.Models.DbModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var myEntity = new MyEntity()
            {
                Id = 1,
                Name = "jjj"
            };
            context.MyEntities.Add(myEntity);
        }
    }
}
