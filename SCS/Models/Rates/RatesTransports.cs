using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class RatesTransports
	{
		[Key]
		public int Id { get; set; }
		public Rates Rates { get; set; }
		public TransportModels TransportModels { get; set; }
	}
}