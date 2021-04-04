using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	public class Payment
	{
		[Key]
		public int Id { get; set; }
		public decimal BonusPayment { get; set; }
		public decimal CardPayment { get; set; }
		public decimal CashPayment { get; set; }
		public decimal CardDeposit { get; set; }
		public decimal CashDeposit { get; set; }
		public decimal TotalSum { get; set; }
		public int TypeDocument { get; set; }

		public ICollection<Order> Orders { get; set; }
		public Payment()
		{
			Orders = new List<Order>();
		}
	}
}