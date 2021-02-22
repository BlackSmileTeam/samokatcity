using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class TypeDocument
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Payment> Payments { get; set; }
		public TypeDocument()
		{
			Payments = new List<Payment>();
		}
	}
}