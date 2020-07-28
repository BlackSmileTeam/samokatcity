using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Rates
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TimeSpan TimeStart { get; set; }
		public TimeSpan TimeEnd { get; set; }
		public int Duration { get; set; }
		public decimal Price { get; set; }


		public Transport Transport { get; set; }
		public int? TransportId { get; set; }	
		public Accessories Accessories { get; set; }
		public int? AccessoriesId { get; set; }


		public ICollection<OrderAccessories> OrderAccessories { get; set; }
		public ICollection<OrderTransport> OrderTransports { get; set; }
		public Rates()
		{
			OrderAccessories = new List<OrderAccessories>();
			OrderTransports = new List<OrderTransport>();
		}
	}
}