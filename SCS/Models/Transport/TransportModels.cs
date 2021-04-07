using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class TransportModels
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal ChargingTime { get; set; } = 0;
		public decimal Markup { get; set; } = 0;

		public List<Transport> Transport { get; set; }
		public List<RatesTransports> RatesTransports { get; set; }
		public List<PromotionsTransportModels> PromotionsTransportModels { get; set; }
		public TransportModels()
		{
			PromotionsTransportModels = new List<PromotionsTransportModels>();
			RatesTransports = new List<RatesTransports>();
			Transport = new List<Transport>();
		}
	}
}