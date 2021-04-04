using System.Collections.Generic;

namespace SCS.Models
{
	public class Rates
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Duration { get; set; }
		public ICollection<RatesTransports> RatesTransports { get; set; }
		public Rates()
		{
			RatesTransports = new List<RatesTransports>();
		}
	}
}