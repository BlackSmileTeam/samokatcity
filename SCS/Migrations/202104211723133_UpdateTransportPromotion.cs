namespace SCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTransportPromotion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Promotions", "DayOfWeek", c => c.String(unicode: false));
            DropColumn("dbo.Transports", "Markup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transports", "Markup", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Promotions", "DayOfWeek", c => c.Int(nullable: false));
        }
    }
}
