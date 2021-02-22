using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class Accessories
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public decimal Price { get; set; } = 0;

		public Accessories()
		{
		}
	}
}