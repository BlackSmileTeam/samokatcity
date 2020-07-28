using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
	public class SCSContext : DbContext
	{
		public SCSContext() : base("conn")
		{ }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Accessories> Accessories { get; set; }
		public DbSet<СontactUser> ContactUser { get; set; }
		public DbSet<OrderAccessories> OrderAccessories { get; set; }
		public DbSet<OrderTransport> OrderTransport { get; set; }
		public DbSet<Payment> Payment { get; set; }
		public DbSet<Rates> Rates { get; set; }
		public DbSet<Transport> Transport { get; set; }
		public DbSet<TypeDocument> TypeDocument { get; set; }
		public DbSet<User> Users { get; set; }
	}

	public class DBInitializzer : DropCreateDatabaseIfModelChanges<SCSContext>
	{
		protected override void Seed(SCSContext sc)
		{
			//sc.User.Add(new User { Username = "test", Password = "test", Bonus = 0, Discount = 0, ExtraCharge = 0});
			//base.Seed(sc);
		}
	}

}