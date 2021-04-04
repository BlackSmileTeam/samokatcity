using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class TransportModels
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal ChargingTime { get; set; } = 0;
		public decimal Markup { get; set; } = 0;

		public ICollection<Transport> Transport { get; set; }
		public ICollection<RatesTransports> RatesTransports { get; set; }
		public ICollection<PromotionsTransportModels> PromotionsTransportModels { get; set; }
		public TransportModels()
		{
			PromotionsTransportModels = new List<PromotionsTransportModels>();
			RatesTransports = new List<RatesTransports>();
			Transport = new List<Transport>();
		}
	}
}