namespace SCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
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
                    ContactUser_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.СontactUser", t => t.ContactUser_Id);

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
                    Note = c.String(unicode: false),
                    Payment_Id = c.Int(),
                    User_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .ForeignKey("dbo.Users", t => t.User_Id);

            CreateTable(
                "dbo.OrderAccessories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Accessories_Id = c.Int(),
                    Order_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accessories", t => t.Accessories_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id);

            CreateTable(
                "dbo.OrderTransports",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Order_Id = c.Int(),
                    Transport_Id = c.Int(),
                    Rates_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Transports", t => t.Transport_Id)
                .ForeignKey("dbo.Rates", t => t.Rates_Id);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RatesTransports",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Rates_Id = c.Int(),
                    TransportModels_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rates", t => t.Rates_Id)
                .ForeignKey("dbo.TransportModels", t => t.TransportModels_Id);
            
            CreateTable(
                "dbo.TransportModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        ChargingTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Markup = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PromotionsTransportModels",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Promotions_Id = c.Int(),
                    TransportModels_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotions", t => t.Promotions_Id)
                .ForeignKey("dbo.TransportModels", t => t.TransportModels_Id);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        DayOfWeek = c.Int(nullable: false),
                        TimeStart = c.Time(nullable: false, precision: 0),
                        TimeEnd = c.Time(nullable: false, precision: 0),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Transports",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    IsBlocked = c.Boolean(nullable: false),
                    StatusId = c.Int(nullable: false),
                    Markup = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SerialNumber = c.String(unicode: false),
                    IndexNumber = c.String(unicode: false),
                    TransportModels_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Helpers", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.TransportModels", t => t.TransportModels_Id);
            
            CreateTable(
                "dbo.Helpers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameHelper = c.String(unicode: false),
                        Code = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        Text = c.String(unicode: false),
                        Queue = c.Int(nullable: false),
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
                        TypeDocument = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.OrderTransports", "Rates_Id", "dbo.Rates");
            DropForeignKey("dbo.Transports", "TransportModels_Id", "dbo.TransportModels");
            DropForeignKey("dbo.Transports", "StatusId", "dbo.Helpers");
            DropForeignKey("dbo.OrderTransports", "Transport_Id", "dbo.Transports");
            DropForeignKey("dbo.RatesTransports", "TransportModels_Id", "dbo.TransportModels");
            DropForeignKey("dbo.PromotionsTransportModels", "TransportModels_Id", "dbo.TransportModels");
            DropForeignKey("dbo.PromotionsTransportModels", "Promotions_Id", "dbo.Promotions");
            DropForeignKey("dbo.RatesTransports", "Rates_Id", "dbo.Rates");
            DropForeignKey("dbo.OrderTransports", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderAccessories", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderAccessories", "Accessories_Id", "dbo.Accessories");
            DropForeignKey("dbo.Users", "ContactUser_Id", "dbo.СontactUser");
            DropIndex("dbo.Transports", new[] { "TransportModels_Id" });
            DropIndex("dbo.Transports", new[] { "StatusId" });
            DropIndex("dbo.PromotionsTransportModels", new[] { "TransportModels_Id" });
            DropIndex("dbo.PromotionsTransportModels", new[] { "Promotions_Id" });
            DropIndex("dbo.RatesTransports", new[] { "TransportModels_Id" });
            DropIndex("dbo.RatesTransports", new[] { "Rates_Id" });
            DropIndex("dbo.OrderTransports", new[] { "Rates_Id" });
            DropIndex("dbo.OrderTransports", new[] { "Transport_Id" });
            DropIndex("dbo.OrderTransports", new[] { "Order_Id" });
            DropIndex("dbo.OrderAccessories", new[] { "Order_Id" });
            DropIndex("dbo.OrderAccessories", new[] { "Accessories_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Orders", new[] { "Payment_Id" });
            DropIndex("dbo.Users", new[] { "ContactUser_Id" });
            DropTable("dbo.Payments");
            DropTable("dbo.Helpers");
            DropTable("dbo.Transports");
            DropTable("dbo.Promotions");
            DropTable("dbo.PromotionsTransportModels");
            DropTable("dbo.TransportModels");
            DropTable("dbo.RatesTransports");
            DropTable("dbo.Rates");
            DropTable("dbo.OrderTransports");
            DropTable("dbo.OrderAccessories");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.СontactUser");
            DropTable("dbo.Accessories");
        }
    }
}
