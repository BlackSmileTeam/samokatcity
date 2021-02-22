using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Transport
	{
		public int Id { get; set; }
		public string Name { get; set; }
		//public int Charge { get; set; }
		public int Status { get; set; }
		public decimal Markup { get; set; } = 0;
		//public Image Icon { get; set; }

		public ICollection<OrderTransport> OrderTransports { get; set; }
		public Transport()
		{
			OrderTransports = new List<OrderTransport>();
		}
	}
}