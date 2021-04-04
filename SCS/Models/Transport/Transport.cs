using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class Transport
	{
		public int Id { get; set; }
		public bool IsBlocked { get; set; } = false;
		public int StatusId { get; set; }
		public decimal Markup { get; set; } = 0;
		public string SerialNumber { get; set; }
		public string IndexNumber { get; set; }
		public TransportModels TransportModels { get; set; }
		public Helpers Status { get; set; }

		public ICollection<OrderTransport> OrderTransports { get; set; }

		public Transport()
		{
			OrderTransports = new List<OrderTransport>();
		}
	}
}