using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class OrderAccessories
	{
		public int Id { get; set; }

		public Order Order { get; set; }

		public Accessories Accessories { get; set; }
	}
}