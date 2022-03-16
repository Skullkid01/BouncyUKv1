namespace BouncyUKv1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        BookingDate = c.DateTime(nullable: false),
                        DeliverAddress = c.String(),
                        PostalCode = c.String(),
                        Cell = c.String(),
                        Time = c.DateTime(nullable: false),
                        PayMtd = c.String(),
                        Refer = c.String(),
                        TotalCost = c.Double(nullable: false),
                        id = c.Int(nullable: false),
                        username = c.String(),
                        BookRef = c.String(),
                    })
                .PrimaryKey(t => t.BookID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Email = c.String(),
                        PayMethod = c.String(),
                        TotalAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.InvID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                        Price = c.Double(nullable: false),
                        Image = c.Binary(),
                        ImageType = c.String(),
                        ProductRef = c.String(),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        CName = c.String(),
                        CSurname = c.String(),
                        Email = c.String(),
                        UName = c.String(),
                        Password = c.String(),
                        CPassword = c.String(),
                    })
                .PrimaryKey(t => t.ClientID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Products");
            DropTable("dbo.Invoices");
            DropTable("dbo.Books");
        }
    }
}
