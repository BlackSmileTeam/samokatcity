using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SCS.Models
{
	public class PromotionsTransportModels
	{
		public int Id { get; set; }
		public Promotions Promotions { get; set; }
		public TransportModels TransportModels { get; set; }
	}
}