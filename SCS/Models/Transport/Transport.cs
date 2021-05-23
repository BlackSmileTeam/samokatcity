using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class Transport
	{
		public int Id { get; set; }
		public bool IsBlocked { get; set; } = false;
		[Display(Name = "Статус")]
		public int Status { get; set; }
		[Display(Name = "Серийный номер")]
		public string SerialNumber { get; set; }
		[Display(Name = "Порядковый номер")]
		public string IndexNumber { get; set; }
		[Display(Name = "Наименование модели")]
		public TransportModels TransportModels { get; set; }
	
		public ICollection<OrderTransport> OrderTransports { get; set; }
		public Transport()
		{
			OrderTransports = new List<OrderTransport>();
		}
	}
}