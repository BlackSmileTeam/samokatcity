using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Rates
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[Display(Name = "Время начала действия тарифа")]

		[DataType(DataType.Time)]
		public TimeSpan TimeStart { get; set; }
		[Display(Name = "Время окончания действия тарифа")]
		[DataType(DataType.Time)]
		public TimeSpan TimeEnd { get; set; }
		public int Duration { get; set; }
		public decimal Price { get; set; }
		public bool IsTransport { get; set; }

		public ICollection<OrderTransport> OrderTransports { get; set; }
		public ICollection<OrderAccessories> OrderAccessories { get; set; }
		public Rates()
		{
			OrderTransports = new List<OrderTransport>();
			OrderAccessories = new List<OrderAccessories>();
		}
	}
}