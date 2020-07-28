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
		public uint Charge { get; set; }
		public bool Status { get; set; }
		//public Image Icon { get; set; }

		public ICollection<OrderTransport> OrderTransports { get; set; }
		public ICollection<Rates> Rates { get; set; }
		public Transport()
		{
			OrderTransports = new List<OrderTransport>();
			Rates = new List<Rates>();
		}
	}
}