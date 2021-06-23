using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class СontactUser
	{
		public int Id { get; set; }
		public string ShortName { get; set; }
		public string Surname { get; set; } = "";
		public string Name { get; set; }
		public string Patronymic { get; set; }
		public string Passport { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string Home { get; set; }
		public string Apartment { get; set; }
		public string Phone { get; set; }
		[JsonIgnore]
		public ICollection<User> Users { get; set; }
		public СontactUser()
		{
			Users = new List<User>();
		}
	}
}