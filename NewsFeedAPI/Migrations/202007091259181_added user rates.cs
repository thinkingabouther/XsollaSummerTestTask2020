namespace NewsFeedAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeduserrates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        NewsInstanceID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRates");
        }
    }
}
