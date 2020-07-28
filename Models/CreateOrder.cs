using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	public class CreateOrder
	{
		public int Id { get; set; }
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateStart { get; set; }
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateEnd { get; set; }
		public int CountLock { get; set; }
		public int Discount { get; set; }
		public int AddBonuses { get; set; }
		public string StatusOrder { get; set; }
		public User User { get; set; }
		public List<Accessories> Accessories { get; set; }
		public List<Rates> RatesAccessories { get; set; }
		public List<Rates> RatesATransports { get; set; }
		public List<Transport> Transports { get; set; }
	}
} 