namespace NewsFeedAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsInstances",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Headline = c.String(),
                    Content = c.String(),
                    RateSum = c.Int(nullable: false),
                    RateCount = c.Int(nullable: false),
                    Category = c.String(),
                })
                .PrimaryKey(t => t.ID);

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
            DropTable("dbo.NewsInstances");
        }
    }
}
