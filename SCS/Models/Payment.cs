using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Payment
	{
		public int Id { get; set; }
		public decimal BonusPayment { get; set; }
		public decimal CardPayment { get; set; }
		public decimal CashPayment { get; set; }
		public decimal CardDeposit { get; set; }
		public decimal CashDeposit { get; set; }
		public decimal TotalSum { get; set; }


		public TypeDocument TypeDocument { get; set; }
		public int? TypeDocumentId { get; set; }


		public ICollection<Order> Orders { get; set; }
		public Payment()
		{
			Orders = new List<Order>();
		}
	}
}