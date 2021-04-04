using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public decimal Discount { get; set; } = 0;
		public decimal Bonus { get; set; } = 0;
		public int ExtraCharge { get; set; } = 0;

		public СontactUser ContactUser { get; set; }
		[JsonIgnore]
		public ICollection<Order> Orders { get; set; }
		public User()
		{
			Orders = new List<Order>();
		}
	}
}