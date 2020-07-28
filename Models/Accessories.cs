using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Accessories
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Status { get; set; }


		public ICollection<OrderAccessories> OrderAccessories { get; set; }
		public ICollection<Rates> Rates { get; set; }
		public Accessories()
		{
			OrderAccessories = new List<OrderAccessories>();
			Rates = new List<Rates>();
		}
	}
}