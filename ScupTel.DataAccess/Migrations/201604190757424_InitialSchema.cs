namespace ScupTel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Charge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinutePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        From_Id = c.Int(),
                        To_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zone", t => t.From_Id)
                .ForeignKey("dbo.Zone", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
            CreateTable(
                "dbo.Zone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscountPlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FreeMinutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Charge", "To_Id", "dbo.Zone");
            DropForeignKey("dbo.Charge", "From_Id", "dbo.Zone");
            DropIndex("dbo.Charge", new[] { "To_Id" });
            DropIndex("dbo.Charge", new[] { "From_Id" });
            DropTable("dbo.DiscountPlan");
            DropTable("dbo.Zone");
            DropTable("dbo.Charge");
        }
    }
}
