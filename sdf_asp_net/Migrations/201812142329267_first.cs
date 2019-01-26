namespace sdf_asp_net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user = c.String(),
                        message = c.String(),
                        dateStamp = c.DateTime(nullable: false),
                        MyProperty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
        }
        
        public override void Down()
        {
            DropTable("dbo.ChatModels");
        }
    }
}
