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
		#region variables
		enum StatusTransportOrAccessories
		{
			Free = 1,
			Busy = 2
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
		private bool createRate
		{
			get
			{
				if (Session["createRate"] != null)
				{
					return Convert.ToBoolean(Session["createRate"]);
				}
				return false;
			}
			set
			{
				Session["createRate"] = value;
			}
		}
		public List<SelectListItem> StatusOrder { get; set; }
		private SCSContext db = new SCSContext();
		#endregion

		public OrdersController()
		{
			StatusOrder = new List<SelectListItem>();

			var ListStatus = db.Helpers.Where(h => h.Code == 2).ToList();
			foreach (var tmpStatus in ListStatus)
			{
				StatusOrder.Add(new SelectListItem { Text = tmpStatus.Text });
			}
		}

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

			Order order = db.Orders.Include(p => p.Payment)
				.Include(u => u.User)
				.Include(u => u.User.ContactUser)
				.Include(ot => ot.OrderTransports.Select(r => r.Rates))
				.Include(tr => tr.OrderTransports.Select(s => s.Transport.TransportModels))
				.Include(oa => oa.OrderAccessories.Select(a => a.Accessories)).FirstOrDefault(o => o.Id == id);
			if (order == null)
			{
				return HttpNotFound();
			}
			order.OrderTransports = order.OrderTransports.DistinctBy(m=>m.Transport.TransportModels).ToList();
			order.OrderAccessories = order.OrderAccessories.DistinctBy(a=>a.Accessories.Name).ToList();

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
			ViewBag.TypeDocumentId = new SelectList(db.Helpers.Where(h => h.Code == 1), "Id", "Name");

			countTransport = 0;
			countAccessories = 0;
			createRate = false;

			List<SelectListItem> TypeDocument = new List<SelectListItem>();

			var status = db.Helpers.Where(statusType => statusType.Code == 1).ToList();
			foreach (var stTr in status)
			{
				TypeDocument.Add(new SelectListItem
				{
					Value = stTr.Value.ToString(),
					Text = stTr.Text
				});
			}
			ViewData["TypeDocumentId"] = TypeDocument;
			return View();
		}

		public ActionResult AddDropListTransport(DateTime dateTime)
		{
			//Добавляем Id для добавленного транспорта
			ViewBag.countTransport = countTransport;
			//Увеличиваем id на единицу, что бы следующий блок был с новым id
			++countTransport;
			Session["countTransport"] = countTransport;
			var rates = db.Rates.ToList();
			ViewBag.RatesIdTransport = new SelectList(rates, "Id", "Name");

			var transp = db.Transport.SqlQuery("CALL transport_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").ToList();

			List<Transport> transports = new List<Transport>();
			foreach (var trans in transp)
			{
				transports.Add(db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == trans.Id));
			}

			ViewBag.TransportId = new SelectList(transports.DistinctBy(tr => tr.TransportModels.Name), "Id", "TransportModels.Name");
			ViewBag.createRate = createRate;
			if (!createRate)
			{
				createRate = true;
			}

			return View(transports.DistinctBy(tr => tr.TransportModels.Name));
		}

		/// <summary>
		/// Получаем количество транспорта возможного для заказа с выбранным названием
		/// </summary>
		/// <param name="nameTransport"></param>
		/// <returns></returns>
		public int MaxCountransport(string nameTransport)
		{
			//return db.Transport.Include(m => m.TransportModels.Name == nameTransport).Include(h => h.Status.Code == 2).Where(h => h.Status.Value == 1).ToList().Count;
			return 0;
		}
		[HttpGet]
		public void DeleteBlockTransport()
		{
			Session["countTransport"] = countTransport - 1;
		}
		public ActionResult AddDropListAccessories(DateTime dateTime)
		{
			//Добавляем Id для добавленного транспорта
			ViewBag.countAccessories = countAccessories;
			//Увеличиваем id на единицу, что бы следующий блок был с новым id
			++countAccessories;
			//var rates = db.Rates.Where(x => x.IsTransport == false).Where(x => x.TimeStart <= dateTime.TimeOfDay && x.TimeEnd >= dateTime.TimeOfDay);
			//ViewBag.RatesIdAccessories = new SelectList(rates, "Id", "Name");

			ViewBag.AccessoriesId = new SelectList(db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").DistinctBy(acc => acc.Name).ToList(), "Id", "Name");

			return View(db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").DistinctBy(acc => acc.Name).ToList());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Create(DateTime DateStart, int CountLock, int UserId,
									   List<int> AccessoriesId, List<int> TransportId, int RatesIdTransport,
									   int addBonuses, int discount, int typeDocumentId, List<int> countTransport, List<int> countAccessories,
									   decimal cashPayment, decimal cardPayment, decimal cardDeposit, decimal cashDeposit, decimal bonusPayment, string Note)
		{

			Order order = new Order();
			if (ModelState.IsValid)
			{
				//Итоговая сумма заказа
				decimal totalSum = 0;

				try
				{
					if (TransportId != null)
					{
						for (int i = 0; i < TransportId.Count; ++i)
						{
							var trId = TransportId[i];
							var modelsId = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == trId).TransportModels.Id;

							totalSum += db.RatesTransports.Include(tm => tm.TransportModels).Include(r => r.Rates).FirstOrDefault(tm => tm.TransportModels.Id == modelsId && tm.Rates.Id == RatesIdTransport).Price * countTransport[i];

							int dayOfWeek = (int)DateStart.DayOfWeek;
							if (countTransport[i] > 0)
							{
								var promotions = db.Promotions.Include(pr => pr.PromotionsTransportModels.Select(tm => tm.TransportModels)).Where(day => day.DayOfWeek.Contains(dayOfWeek.ToString())).ToList();

								foreach (var promotion in promotions)
								{
									foreach (var model in promotion.PromotionsTransportModels)
									{
										if (model.TransportModels != null && model.TransportModels.Id == modelsId)
										{
											totalSum -= promotion.Discount;
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				totalSum += CountLock * 100;
				var remainder = totalSum - cardDeposit - cardPayment - cashDeposit - cashPayment - bonusPayment - discount;

				order.StatusOrder = DateStart >= DateTime.Now.AddHours(1) ? (remainder == 0 ? "Забронирован (Оплачен)" : "Забронирован (Ожидает оплаты)") : (remainder == 0 ? "В поездке (Оплачен)" : "В поездке (Ожидает оплаты)");
				int statusTransportOrAccesories = DateStart >= DateTime.Now.AddHours(1) ? Convert.ToInt32(StatusTransportOrAccessories.Free) : Convert.ToInt32(StatusTransportOrAccessories.Busy);

				order.DateStart = DateStart;
				order.DateEnd = DateStart.AddHours(db.Rates.Find(RatesIdTransport) != null ? db.Rates.Find(RatesIdTransport).Duration : 0);
				order.CountLock = CountLock;
				order.User = db.Users.Find(UserId);
				order.Discount = discount;
				order.AddBonuses = addBonuses;
				order.Note = Note;

				db.Orders.Add(order);
				db.SaveChanges();

				db.Users.Find(UserId).Bonus += addBonuses - bonusPayment;
				var rate = db.Rates.Find(RatesIdTransport);

				if (TransportId != null)
				{
					for (int i = 0; i < TransportId.Count; ++i)
					{
						//Ищем данные о выбранном тарифе для добавление к заказу и поиску свободных в определенное время

						var idTransport = TransportId[i];
						var transModel = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == idTransport).TransportModels.Id;
						//ищем ТС которые подходят в выбранную дату (свободны) и определенное количество
						var transp = db.Transport.SqlQuery("CALL transport_date_vw('" + DateStart.ToString("yyyy-MM-dd HH:mm") + "','" + DateStart.AddHours(rate.Duration).ToString("yyyy-MM-dd HH:mm") + "')").Take(countTransport[i]).ToList();

						List<Transport> transports = new List<Transport>();
						foreach (var trans in transp)
						{
							//из полученного списка ранее ищем еще те которые подходят нам по модели
							transports.Add(db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == trans.Id && tr.TransportModels.Id == transModel));
						}
						foreach (var tr in transports)
						{
							//меняем статус, если это необходимо
							//tr.Status = statusTransportOrAccesories;
							db.Entry(tr).State = EntityState.Modified;
						}

						db.SaveChanges();

						foreach (var tr in transports)
						{
							db.OrderTransport.Add(new OrderTransport()
							{
								Transport = tr,
								Order = order,
								Rates = rate
							});
						}
						db.SaveChanges();
					}
				}

				if (AccessoriesId != null)
				{
					for (int i = 0; i < AccessoriesId.Count; ++i)
					{
						var idAccesories = AccessoriesId[i];
						var accName = db.Accessories.FirstOrDefault(ac => ac.Id == idAccesories).Name;
						var access = db.Accessories.SqlQuery("CALL accessories_date_vw('" + DateStart.ToString("yyyy-MM-dd HH:mm") + "','" + DateStart.AddHours(rate.Duration).ToString("yyyy-MM-dd HH:mm") + "')").Take(countAccessories[i]).ToList();

						List<Accessories> accessories = new List<Accessories>();
						foreach (var acc in access)
						{
							//из полученного списка ранее ищем еще те которые подходят нам по модели
							accessories.Add(db.Accessories.FirstOrDefault(ac => ac.Id == acc.Id && ac.Name == accName));
						}
						foreach (var ac in accessories)
						{
							//меняем статус, если это необходимо
							//ac.Status = statusTransportOrAccesories;
							db.Entry(ac).State = EntityState.Modified;
						}

						db.SaveChanges();

						foreach (var ac in accessories)
						{
							db.OrderAccessories.Add(new OrderAccessories()
							{
								Accessories = ac,
								Order = order,
							});
						}
						db.SaveChanges();
					}
				}

				var tdoc = db.Helpers.Where(h => h.Code == 1 && h.Value == typeDocumentId).ToList();
				Payment payment = new Payment()
				{
					TypeDocument = tdoc != null && tdoc.Count > 0 ? tdoc[0].Value : 3,
					CashPayment = cashPayment,
					CardPayment = cardPayment,
					CardDeposit = cardDeposit,
					CashDeposit = cashDeposit,
					BonusPayment = bonusPayment,
					TotalSum = totalSum
				};
				db.Payment.Add(payment);
				db.Orders.Find(order.Id).Payment = payment;

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

			//var Payment = db.Payment.Where(p => p.Id == order.PaymentId).ToList()[0];
			//var orderTransport = db.OrderTransport.Where(o => o.OrderId == id).Include(t => t.Rates).Include(tr => tr.Transport);
			//var orderAccessories = db.OrderAccessories.Where(o => o.OrderId == id).Include(ac => ac.Accessories);

			//foreach (var ot in orderTransport)
			//{
			//	//TotalSum += ot.Rates.Price;
			//	if (weekend)
			//	{
			//		TotalSum += ot.Transport.Markup;
			//	}
			//}
			//foreach (var oa in orderAccessories)
			//{
			//	//TotalSum += oa.Rates.Price;
			//}

			TotalSum += order.CountLock * 100;

			ViewBag.TypeDocumentId = new SelectList(db.Helpers.Where(h => h.Code == 1), "Id", "Name");
			//ViewBag.UserId = new SelectList(db.Users, "Id", "Username", order.UserId);
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

				order.StatusOrder = order.StatusOrder.Remove(order.StatusOrder.IndexOf('(') + 1);

				order.StatusOrder += remainder == 0 ? "Оплачен)" : "Ожидает оплаты)";

				order.Discount = Convert.ToInt32(Discount);
				pay.CashPayment = Convert.ToDecimal(CashPayment);
				pay.CashDeposit = Convert.ToDecimal(CashDeposit);
				pay.CardPayment = Convert.ToDecimal(CardPayment);
				pay.CardDeposit = Convert.ToDecimal(CardDeposit);
				pay.TypeDocument = TypeDocumentId;
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
			Order order = db.Orders.Include(u => u.User).Include(p => p.Payment).FirstOrDefault(o => o.Id == id);
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
			Order order = db.Orders.Include(u => u.User).Include(p => p.Payment).FirstOrDefault(o => o.Id == id);
			List<OrderAccessories> orderAccessories = db.OrderAccessories.Include(o => o.Order).Where(oa => oa.Order.Id == order.Id).ToList();
			List<OrderTransport> orderTransports = db.OrderTransport.Include(o => o.Order).Where(oa => oa.Order.Id == order.Id).ToList();

			var user = db.Users.Find(order.User.Id);

			foreach (var oa in orderAccessories)
			{
				db.OrderAccessories.Remove(oa);
			}
			foreach (var ot in orderTransports)
			{
				db.OrderTransport.Remove(ot);
			}

			db.Orders.Remove(order);

			if (order.Payment != null)
			{
				user.Bonus -= order.AddBonuses + order.Payment.BonusPayment;
				db.Entry(user).State = EntityState.Modified;
				db.Payment.Remove(order.Payment);
			}

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
