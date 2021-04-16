using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class TransportModels
	{
		public int Id { get; set; }

		[Display(Name = "Наименование")]
		public string Name { get; set; }

		[Display(Name = "Время зарядки (ч)")]
		public decimal ChargingTime { get; set; } = 0;
		[Display(Name = "Наценка (руб)")]
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