using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class Promotions
	{
		public int Id { get; set; }
		[Display(Name = "Наименование")]
		public string Name { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "День недели")]
		public string DayOfWeek { get; set; }
		[Display(Name = "Время начала действия")]
		public TimeSpan TimeStart { get; set; }
		[Display(Name = "Время окончания действия")]
		public TimeSpan TimeEnd { get; set; }
		[Display(Name = "Скидка")]
		public decimal Discount { get; set; }
		public List<PromotionsTransportModels> PromotionsTransportModels { get; set; }
		public Promotions()
		{
			PromotionsTransportModels = new List<PromotionsTransportModels>();
		}
	}
}