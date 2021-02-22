namespace SCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accessories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.СontactUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(unicode: false),
                        Surname = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Patronymic = c.String(unicode: false),
                        Passport = c.String(unicode: false),
                        City = c.String(unicode: false),
                        Street = c.String(unicode: false),
                        Home = c.String(unicode: false),
                        Apartment = c.String(unicode: false),
                        Phone = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Username = c.String(unicode: false),
                    Password = c.String(unicode: false),
                    Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Bonus = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ExtraCharge = c.Int(nullable: false),
                    ContactUserId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.СontactUser", t => t.ContactUserId);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateStart = c.DateTime(nullable: false, precision: 0),
                    DateEnd = c.DateTime(nullable: false, precision: 0),
                    CountLock = c.Int(nullable: false),
                    Discount = c.Int(nullable: false),
                    AddBonuses = c.Int(nullable: false),
                    StatusOrder = c.String(unicode: false),
                    PaymentId = c.Int(),
                    UserId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .ForeignKey("dbo.Users", t => t.UserId);

            CreateTable(
                "dbo.OrderTransports",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OrderId = c.Int(),
                    RatesId = c.Int(),
                    TransportId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Rates", t => t.RatesId)
                .ForeignKey("dbo.Transports", t => t.TransportId);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        TimeStart = c.Time(nullable: false, precision: 0),
                        TimeEnd = c.Time(nullable: false, precision: 0),
                        Duration = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsTransport = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        Markup = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Payments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BonusPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CardPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CashPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CardDeposit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CashDeposit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TotalSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TypeDocumentId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeDocuments", t => t.TypeDocumentId);
            
            CreateTable(
                "dbo.TypeDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Payments", "TypeDocumentId", "dbo.TypeDocuments");
            DropForeignKey("dbo.Orders", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.OrderTransports", "TransportId", "dbo.Transports");
            DropForeignKey("dbo.OrderTransports", "RatesId", "dbo.Rates");
            DropForeignKey("dbo.OrderTransports", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Users", "ContactUserId", "dbo.СontactUser");
            DropIndex("dbo.Payments", new[] { "TypeDocumentId" });
            DropIndex("dbo.OrderTransports", new[] { "TransportId" });
            DropIndex("dbo.OrderTransports", new[] { "RatesId" });
            DropIndex("dbo.OrderTransports", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "PaymentId" });
            DropIndex("dbo.Users", new[] { "ContactUserId" });
            DropTable("dbo.TypeDocuments");
            DropTable("dbo.Payments");
            DropTable("dbo.Transports");
            DropTable("dbo.Rates");
            DropTable("dbo.OrderTransports");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.СontactUser");
            DropTable("dbo.Accessories");
        }
    }
}
