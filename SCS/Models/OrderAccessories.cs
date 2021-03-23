using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class OrderAccessories
	{
		public int Id { get; set; }

		public Order Order { get; set; }
		public int? OrderId { get; set; }

		public Rates Rates { get; set; }
		public int? RatesId { get; set; }

		public Accessories Accessories { get; set; }
		public int? AccessoriesId { get; set; }
	}
}