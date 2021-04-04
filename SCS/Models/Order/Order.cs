using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCS.Models
{
	/// <summary>
	/// Текущие заказы в работе
	/// </summary>
	public class Order
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Дата и время начала")]
		[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateStart { get; set; }

		[Display(Name = "Дата и время конца")]
		[DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime DateEnd { get; set; }
		[Display(Name = "Количество замков")]
		public int CountLock { get; set; }
		[Display(Name = "Скидка")]
		public int Discount { get; set; }
		[Display(Name = "Количество добавленных бонусов")]
		public int AddBonuses { get; set; }
		[Display(Name = "Статус заказа")]
		public string StatusOrder { get; set; }	

		[Display(Name = "Сумма заказа")]
		public Payment Payment { get; set; }

		[Display(Name = "Пользователь")]
		public User User { get; set; }
		[Display( Name = "Примечание к заказу")]
		public string Note { get; set; }
		public ICollection<OrderTransport> OrderTransports { get; set; }
		public ICollection<OrderAccessories> OrderAccessories { get; set; }
		public Order()
		{
			OrderTransports = new List<OrderTransport>();
			OrderAccessories = new List<OrderAccessories>();
		}
	}
}



