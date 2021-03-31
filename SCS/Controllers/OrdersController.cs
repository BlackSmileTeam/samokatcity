using Microsoft.Ajax.Utilities;
using SCS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SCS.Controllers
{
	public class OrdersController : Controller
	{
		enum StatusTransportOrAccessories
		{
			Free,
			Busy
		}
		//Количество добавленного транспорта
		private int countTransport
		{
			get
			{
				if (Session["countTransport"] != null)
				{
					return Convert.ToInt32(Session["countTransport"]);
				}
				return 0;
			}
			set
			{
				Session["countTransport"] = value;
			}
		}
		private int countAccessories
		{
			get
			{
				if (Session["countAccessories"] != null)
				{
					return Convert.ToInt32(Session["countAccessories"]);
				}
				return 0;
			}
			set
			{
				Session["countAccessories"] = value;
			}
		}
		private List<string> statusOrder = new List<string>
		{
			"Все","Забронирован","Оплачен","Отменен","В поездке"
		};
		public List<SelectListItem> StatusOrder { get; set; }
		public OrdersController()
		{
			StatusOrder = new List<SelectListItem>();

			foreach (string tmpStatus in statusOrder)
			{
				StatusOrder.Add(new SelectListItem { Text = tmpStatus });
			}
		}

		private SCSContext db = new SCSContext();

		// GET: Orders
		public ActionResult Index()
		{
			var orders = db.Orders.Include(u => u.User).Include(cu => cu.User.ContactUser).Include(p => p.Payment);//.Take(10);
			ViewBag.StatusOrder = StatusOrder;
			SelectList users = new SelectList(db.Users.Include(u => u.ContactUser), "Id", "Id");
			ViewBag.User = users;
			countTransport = 0;
			countAccessories = 0;
			return View(orders.ToList());
		}

		/// <summary>
		/// Фильтрация задач
		/// </summary>
		/// <param name="status"></param>
		/// <param name="dateStart"></param>
		/// <param name="dateEnd"></param>
		/// <returns></returns>
		public ActionResult Filter(string statusOrder, DateTime dateStart, DateTime dateEnd)
		{
			var orders = db.Orders.Include(u => u.User).Include(cu => cu.User.ContactUser).Include(p => p.Payment).Where(o => o.DateStart >= dateStart && o.DateStart <= dateEnd).ToList();

			if (statusOrder != "Все")
			{
				orders = orders.Where(o => o.StatusOrder.Contains(statusOrder)).ToList();
			}

			ViewBag.StatusOrder = StatusOrder;

			return PartialView(orders);
		}

		// GET: Orders/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Order order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			order.User = db.Users.Find(order.UserId);
			if (order.User != null)
			{
				//Получаем подробные данные о пользователе
				order.User.ContactUser = db.ContactUser.Find(order.User.ContactUserId);
			}
			//Получаем связанные данные
			order.OrderTransports = db.OrderTransport.Include(ot => ot.Rates).Include(ot => ot.Transport).Where(ot => ot.OrderId == id).ToList();
			order.OrderAccessories = db.OrderAccessories.Include(ot => ot.Rates).Include(oa => oa.Accessories).Where(ot => ot.OrderId == id).ToList();

			order.Payment = db.Payment.Include(p => p.TypeDocument).FirstOrDefault(s => s.Id == order.PaymentId);

			return PartialView(order);
		}

		// GET: Orders/Create
		public ActionResult Create()
		{
			ViewBag.UserId = new SelectList(db.Users.Include(u => u.ContactUser), "Id", "ContactUser.ShortName");
			ViewBag.ContactUser = db.ContactUser;
			ViewBag.StatusOrder = StatusOrder;
			ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name");
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name");
			ViewBag.OrderId = new SelectList(db.Orders, "Id", "StatusOrder");
			ViewBag.RatesId = new SelectList(db.Rates, "Id", "Name");
			ViewBag.TypeDocumentId = new SelectList(db.TypeDocument, "Id", "Name");

			countTransport = 0;
			countAccessories = 0;
			return View();
		}

		public ActionResult AddDropListTransport(DateTime dateTime)
		{
			//Добавляем Id для добавленного транспорта
			ViewBag.countTransport = countTransport;
			//Увеличиваем id на единицу, что бы следующий блок был с новым id
			++countTransport;
			Session["countTransport"] = countTransport;
			var rates = db.Rates.Where(x => x.IsTransport == true).Where(x => x.TimeStart <= dateTime.TimeOfDay && x.TimeEnd >= dateTime.TimeOfDay);
			ViewBag.RatesIdTransport = new SelectList(rates, "Id", "Name");

			ViewBag.TransportId = new SelectList(db.Transport.SqlQuery("CALL transport_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").DistinctBy(tr => tr.Id).ToList(), "Id", "Name");

			return View(db.Transport.SqlQuery("CALL transport_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").ToList().DistinctBy(tr => tr.Id));
		}

		/// <summary>
		/// Получаем количество транспорта возможного для заказа с выбранным названием
		/// </summary>
		/// <param name="nameTransport"></param>
		/// <returns></returns>
		public int MaxCountransport(string nameTransport)
		{
			return db.Transport.Where(tr => tr.Name == nameTransport && tr.Status == 1).ToList().Count;
		}

		public ActionResult AddDropListAccessories(DateTime dateTime)
		{
			//Добавляем Id для добавленного транспорта
			ViewBag.countAccessories = countAccessories;
			//Увеличиваем id на единицу, что бы следующий блок был с новым id
			++countAccessories;
			var rates = db.Rates.Where(x => x.IsTransport == false).Where(x => x.TimeStart <= dateTime.TimeOfDay && x.TimeEnd >= dateTime.TimeOfDay);
			ViewBag.RatesIdAccessories = new SelectList(rates, "Id", "Name");

			ViewBag.AccessoriesId = new SelectList(db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").DistinctBy(acc => acc.Id).ToList(), "Id", "Name");

			return View(db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").DistinctBy(acc => acc.Id).ToList());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Create(DateTime DateStart, int CountLock, int UserId,
									   List<int> AccessoriesId, List<int> TransportId, List<int> RatesIdTransport, List<int> RatesIdAccessories,
									   int addBonuses, int discount, int typeDocumentId,
									   decimal cashPayment, decimal cardPayment, decimal cardDeposit, decimal cashDeposit, decimal bonusPayment)
		{

			Order order = new Order();
			if (ModelState.IsValid)
			{
				//Итоговая сумма заказа
				decimal totalSum = 0;
				//проверка на выходной день
				bool weekend = false;
				if (DateStart.DayOfWeek.ToString().ToLower() == "суббота" || DateStart.DayOfWeek.ToString().ToLower() == "воскресенье" ||
					DateStart.DayOfWeek.ToString().ToLower() == "saturday" || DateStart.DayOfWeek.ToString().ToLower() == "sunday")
				{
					weekend = true;
				}
				if (weekend)
				{
					foreach (var transp in TransportId)
					{
						totalSum += db.Transport.Find(transp).Markup;
					}
				}
				if(RatesIdTransport != null)
				{
					foreach (var Rate in RatesIdTransport)
					{
						totalSum += db.Rates.Find(Rate).Price;
					}
				}
				if (RatesIdAccessories != null)
				{
					foreach (var Rate in RatesIdAccessories)
					{
						totalSum += db.Rates.Find(Rate).Price;
					}
				}

				totalSum += CountLock * 100;
				var remainder = totalSum - cardDeposit - cardPayment - cashDeposit - cashPayment - bonusPayment - discount;

				order.StatusOrder = DateStart >= DateTime.Now.AddHours(1) ? (remainder == 0 ? "Забронирован (Оплачен)" : "Забронирован (Ожидает оплаты)") : (remainder == 0 ? "В поездке (Оплачен)" : "В поездке (Ожидает оплаты)");
				int statusTransportOrAccesories = DateStart >= DateTime.Now.AddHours(1) ? Convert.ToInt32(StatusTransportOrAccessories.Free) : Convert.ToInt32(StatusTransportOrAccessories.Busy);

				order.DateStart = DateStart;
				order.CountLock = CountLock;
				order.UserId = UserId;
				order.User = db.Users.Find(UserId);
				order.Discount = discount;
				order.AddBonuses = addBonuses;

				db.Orders.Add(order);
				db.SaveChanges();

				db.Users.Find(UserId).Bonus += addBonuses - bonusPayment;

				if (TransportId != null)
				{
					for (int i = 0; i < TransportId.Count; ++i)
					{
						var idTransport = TransportId[i];
						var idRate = RatesIdTransport[i];
						var trans = db.Transport.FirstOrDefault(tr => tr.Id == idTransport);
						if (DateStart <= DateTime.Now.AddHours(1))
						{
							trans.Status = 0;
							db.Entry(trans).State = EntityState.Modified;
							db.SaveChanges();
						}
						db.OrderTransport.Add(new OrderTransport()
						{
							Transport = db.Transport.Find(idTransport),
							TransportId = idTransport,
							OrderId = order.Id,
							RatesId = idRate,
							Rates = db.Rates.Find(idRate)
						});
					}
				}

				if (AccessoriesId != null)
				{
					for (int i = 0; i < AccessoriesId.Count; ++i)
					{
						var idAccessories = AccessoriesId[i];
						var rateId = RatesIdAccessories[i];
						var access = db.Accessories.FirstOrDefault(acc => acc.Id == idAccessories);
						if (DateStart <= DateTime.Now.AddHours(1))
						{
							access.Status = 0;
							db.Entry(access).State = EntityState.Modified;
							db.SaveChanges();
						}
						db.OrderAccessories.Add(new OrderAccessories()
						{
							Accessories = db.Accessories.Find(idAccessories),
							AccessoriesId = idAccessories,
							OrderId = order.Id,
							RatesId = rateId,
							Rates = db.Rates.Find(rateId)
						});
					}
				}


				Payment payment = new Payment()
				{
					TypeDocumentId = typeDocumentId,
					TypeDocument = db.TypeDocument.Find(typeDocumentId),
					CashPayment = cashPayment,
					CardPayment = cardPayment,
					CardDeposit = cardDeposit,
					CashDeposit = cashDeposit,
					BonusPayment = bonusPayment,
					TotalSum = totalSum
				};
				db.Payment.Add(payment);
				db.SaveChanges();


				//Устанавливаем конец заказа на самый последний день использования транспорта или аксессуара
				db.Orders.Find(order.Id).Payment = payment;
				db.Orders.Find(order.Id).PaymentId = payment.Id;

				db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(order);
		}

		// GET: Orders/Edit/5
		public ActionResult Edit(int? id)
		{
			decimal TotalSum = 0;
			bool weekend = false;

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Include(p => p.Payment).FirstOrDefault(p => p.Id == id);
			if (order == null)
			{
				return HttpNotFound();
			}
			if (order.DateStart.DayOfWeek.ToString().ToLower() == "суббота" || order.DateStart.DayOfWeek.ToString().ToLower() == "воскресенье" ||
				order.DateStart.DayOfWeek.ToString().ToLower() == "saturday" || order.DateStart.DayOfWeek.ToString().ToLower() == "sunday")
			{
				weekend = true;
			}
			var Payment = db.Payment.Where(p => p.Id == order.PaymentId).ToList()[0];
			var orderTransport = db.OrderTransport.Where(o => o.OrderId == id).Include(t => t.Rates).Include(tr => tr.Transport);
			var orderAccessories = db.OrderAccessories.Where(o => o.OrderId == id).Include(t => t.Rates).Include(ac => ac.Accessories);

			foreach (var ot in orderTransport)
			{
				TotalSum += ot.Rates.Price;
				if (weekend)
				{
					TotalSum += ot.Transport.Markup;
				}
			}
			foreach (var oa in orderAccessories)
			{
				TotalSum += oa.Rates.Price;
			}

			TotalSum += order.CountLock * 100;

			ViewBag.TypeDocumentId = new SelectList(db.TypeDocument, "Id", "Name");
			ViewBag.UserId = new SelectList(db.Users, "Id", "Username", order.UserId);
			ViewBag.TotalSum = TotalSum.ToString();
			return View(order);
		}

		// POST: Orders/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int Id, decimal? CashPayment, decimal? CashDeposit, decimal? CardPayment, decimal? CardDeposit, int? Discount, int TypeDocumentId, decimal? bonusPayment)
		{
			CashPayment = CashPayment == null ? 0 : CashPayment;
			CashDeposit = CashDeposit == null ? 0 : CashDeposit;
			CardPayment = CardPayment == null ? 0 : CardPayment;
			CardDeposit = CardDeposit == null ? 0 : CardDeposit;
			bonusPayment = bonusPayment == null ? 0 : bonusPayment;
			Discount = Discount == null ? 0 : Discount;

			Payment pay = new Payment();
			if (ModelState.IsValid)
			{
				Order order = new Order();
				order = db.Orders.Include(p => p.Payment).FirstOrDefault(p => p.Id == Id);
				pay = order.Payment;
				var remainder = pay.TotalSum - CardDeposit - CardPayment - CashDeposit - CashPayment - bonusPayment - Discount;

				order.StatusOrder = order.StatusOrder.Remove(order.StatusOrder.IndexOf('(')+1);

				order.StatusOrder += remainder == 0 ?  "Оплачен)" : "Ожидает оплаты)";

				order.Discount = Convert.ToInt32(Discount);				
				pay.CashPayment = Convert.ToDecimal(CashPayment);
				pay.CashDeposit = Convert.ToDecimal(CashDeposit);
				pay.CardPayment = Convert.ToDecimal(CardPayment);
				pay.CardDeposit = Convert.ToDecimal(CardDeposit);
				pay.TypeDocumentId = TypeDocumentId;
				pay = order.Payment;

				db.Entry(pay).State = EntityState.Modified;
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(pay);
		}

		// GET: Orders/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Include(u=>u.User).Include(p=>p.Payment).FirstOrDefault(o=>o.Id == id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Order order = db.Orders.Find(id);
			Payment pay = db.Payment.Find(order.PaymentId);
			List<OrderAccessories> orderAccesories = db.OrderAccessories.Where(oa=>oa.OrderId == order.Id).ToList();
			List<OrderTransport> orderTransport = db.OrderTransport.Where(oa => oa.OrderId == order.Id).ToList();
			foreach (var OA in orderAccesories)
			{
				db.OrderAccessories.Remove(OA);
			}
			foreach (var OT in orderTransport)
			{
				db.OrderTransport.Remove(OT);
			}
		
			db.Orders.Remove(order);
			db.Payment.Remove(pay);

			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
