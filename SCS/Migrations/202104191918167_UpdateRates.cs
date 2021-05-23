namespace SCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RatesTransports", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RatesTransports", "Price");
        }
    }
}
