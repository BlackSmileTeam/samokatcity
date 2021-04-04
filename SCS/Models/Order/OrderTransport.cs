using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class OrderTransport
	{
		[Key]
		public int Id { get; set; }

		public Order Order { get; set; }

		public Rates Rates { get; set; }

		public Transport Transport { get; set; }
	}
}