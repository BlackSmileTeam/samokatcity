using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class Rates
	{
		public int Id { get; set; }
		[Display(Name = "Наименование")]
		public string Name { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; }
		[Display(Name = "Продолжительность (ч)")]
		public int Duration { get; set; }
		public List<RatesTransports> RatesTransports { get; set; }
		public Rates()
		{
			RatesTransports = new List<RatesTransports>();
		}
	}
}