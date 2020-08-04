﻿using SCS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace SCS.Controllers
{
	public class OrdersController : Controller
	{
		private List<string> statusOrder = new List<string>
		{
			"Забронирован","Оплачен","Просрочен","Отменен"
		};
		public List<SelectListItem> StatusOrder { get; set; }
		public OrdersController()
		{
			StatusOrder = new List<SelectListItem>();

			foreach (string tmpStatus in statusOrder)
			{
				StatusOrder.Add(new SelectListItem { Text = tmpStatus});
			}
		}

		private SCSContext db = new SCSContext();

		// GET: Orders
		public ActionResult Index()
		{
			var orders = db.Orders.Include(u => u.User).Include(cu => cu.User.ContactUser).Where(o=>o.StatusOrder == "Забронирован");
			ViewBag.StatusOrder = StatusOrder;

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
			var orders = db.Orders.Include(u => u.User).Include(cu => cu.User.ContactUser).Where(o => o.DateStart >= dateStart && o.DateEnd <= dateEnd && o.StatusOrder == statusOrder).ToList();

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
			order.OrderAccessories = db.OrderAccessories.Include(oa => oa.Rates).Include(oa => oa.Accessories).Where(oa => oa.OrderId == id).ToList();
			order.OrderTransports = db.OrderTransport.Include(ot => ot.Rates).Include(ot => ot.Transport).Where(ot => ot.OrderId == id).ToList();

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

			return View();
		}

		public ActionResult AddDropListTransport()
		{
			var rates = db.Rates.Include(tr => tr.Transport).Where(tr => tr.Transport != null);
			ViewBag.RatesId = new SelectList(rates, "Id", "Name");
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name");
			return View();
		}
		public ActionResult AddDropListAccessories()
		{
			var rates = db.Rates.Include(ac => ac.Accessories).Where(ac => ac.Accessories != null);
			ViewBag.RatesId = new SelectList(rates, "Id", "Name");
			ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(DateTime DateStart, int CountLock, int StatusOrder, int UserId,
								   List<int> AccessoriesId, List<int> TransportId, List<int> RatesId,
								   List<int> countTransport, List<int> countAccessories,
								   int addBonuses, int discount, int typeDocumentId,
								   decimal cashPayment, decimal cardPayment, decimal cardDeposit, decimal cashDeposit, decimal bonusPayment)
		{
			Order order = new Order();
			if (ModelState.IsValid)
			{
				//Итоговая сумма заказа
				decimal totalSum = 0;
				order.DateStart = DateStart;
				order.CountLock = CountLock;
				order.StatusOrder = statusOrder[StatusOrder];
				order.UserId = UserId;
				order.User = db.Users.Find(UserId);
				order.Discount = discount;
				order.AddBonuses = addBonuses;

				db.Users.Find(UserId).Bonus += addBonuses;

				db.Orders.Add(order);
				db.SaveChanges();


				int duration = 0;
				for (int i = 0; i < AccessoriesId.Count; ++i)
				{
					int tmpDuration = db.Rates.Find(RatesId[i]).Duration;
					if (tmpDuration > duration)
					{
						duration = tmpDuration;
					}
					//Добавляем введенное количество аксессуаров пользователем
					for (int j = 0; j < countAccessories[i]; ++j)
					{
						//Увеличиваем сумму заказа
						totalSum += db.Rates.Find(RatesId[i]).Price;
						db.OrderAccessories.Add(new OrderAccessories()
						{
							Accessories = db.Accessories.Find(AccessoriesId[i]),
							AccessoriesId = AccessoriesId[i],
							OrderId = order.Id,
							RatesId = RatesId[i],
							Rates = db.Rates.Find(RatesId[i])
						});
					}
				}

				for (int i = 0; i < TransportId.Count; ++i)
				{
					int tmpDuration = db.Rates.Find(RatesId[i]).Duration;
					if (tmpDuration > duration)
					{
						duration = tmpDuration;
					}
					//Добавляем введенное количество транспорта пользователем					
					for (int j = 0; j < countTransport[i]; ++j)
					{
						//Увеличиваем сумму заказа
						totalSum += db.Rates.Find(RatesId[i]).Price;
						db.OrderTransport.Add(new OrderTransport()
						{
							Transport = db.Transport.Find(TransportId[i]),
							TransportId = TransportId[i],
							OrderId = order.Id,
							RatesId = RatesId[i],
							Rates = db.Rates.Find(RatesId[i])
						});
					}
				}
				//Счиатем скидку пользователя
				totalSum = totalSum - totalSum / 100 * discount;

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
				//Вычитаем оплаченные бонусы из общего количества, которое имеется у пользователя
				db.Users.Find(UserId).Bonus -= bonusPayment;
				db.Payment.Add(payment);
				db.SaveChanges();


				//Устанавливаем конец заказа на самый последний день использования транспорта или аксессуара
				db.Orders.Find(order.Id).DateEnd = DateStart.AddDays(duration);
				db.Orders.Find(order.Id).Payment = payment;
				db.Orders.Find(order.Id).PaymentId = payment.Id;

				db.SaveChanges();

				db.SaveChanges();
				return RedirectToAction("Index");
			}

			//ViewBag.UserId = new SelectList(db.Users, "Id", "Username", order.UserId);
			//return View(order);
			return View(order);
		}

		// GET: Orders/Edit/5
		public ActionResult Edit(int? id)
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
			ViewBag.UserId = new SelectList(db.Users, "Id", "Username", order.UserId);
			return View(order);
		}

		// POST: Orders/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,DateStart,DateEnd,CountLock,StatusOrder,UserId")] Order order)
		{
			if (ModelState.IsValid)
			{
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.UserId = new SelectList(db.Users, "Id", "Username", order.UserId);
			return View(order);
		}

		// GET: Orders/Delete/5
		public ActionResult Delete(int? id)
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
			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Order order = db.Orders.Find(id);
			db.Orders.Remove(order);
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