namespace AspNet.WebApi.ConnectDbByEF.MySql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMyEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MyEntities");
        }
    }
}
