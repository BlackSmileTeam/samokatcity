using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCS.Models
{
	/// <summary>
	/// Текущие заказы в работе
	/// </summary>
	public class Order
	{
		public int Id { get; set; }
		[Display(Name = "Дата и время начала")]
		[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateStart { get; set; }

		[Display(Name = "Дата и время конца")]
		[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateEnd { get; set; }
		public int CountLock { get; set; }
		public int Discount { get; set; }
		public int AddBonuses { get; set; }
		public string StatusOrder { get; set; }	

		public int? PaymentId { get; set; }
		public Payment Payment { get; set; }

		public int? UserId { get; set; }
		public User User { get; set; }

		public ICollection<OrderTransport> OrderTransports { get; set; }
		public Order()
		{
			OrderTransports = new List<OrderTransport>();
		}
	}
}



