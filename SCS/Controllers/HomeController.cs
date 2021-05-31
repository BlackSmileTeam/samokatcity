using SCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;
using System.Diagnostics;


namespace SCS.Controllers
{
	public class HomeController : Controller
	{
		SCSContext db = new SCSContext();
		public ActionResult Index()
		{
			decimal YesterdayProfit = 0;
			decimal YesterdayCashPaymentProfit = 0;
			decimal YesterdayCardPaymentProfit = 0;
			decimal YesterdayBonusPaymentProfit = 0;

			decimal TodayProfit = 0;
			decimal TodayCashPaymentProfit = 0;
			decimal TodayCardPaymentProfit = 0;
			decimal TodayBonusPaymentProfit = 0;

			var dateYesterday = DateTime.Today.AddDays(-1);


			var FreeTransport = db.Transport.SqlQuery("CALL transport_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList();
			var FreeAccessories = db.Accessories.SqlQuery("CALL accessories_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList();

			List<Transport> transports = new List<Transport>();
			foreach (var trans in FreeTransport)
			{
				//из полученного списка ранее ищем еще те которые подходят нам по модели
				transports.Add(db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == trans.Id));
			}

			var transportModels = new Dictionary<string, int>();
			foreach (var ftr in transports)
			{
				if (transportModels.ContainsKey(ftr.TransportModels.Name))
				{
					transportModels[ftr.TransportModels.Name]++;
				}
				else
				{
					transportModels.Add(ftr.TransportModels.Name, 1);
				}
			}

			string FreeTransportTitle = "";
			foreach (var ft in transportModels)
			{
				FreeTransportTitle += ft.Key + " - " + ft.Value + "\n";
			}

			ViewData["FreeTransportTitle"] = FreeTransportTitle;

			List<Accessories> accessories = new List<Accessories>();
			foreach (var access in FreeAccessories)
			{
				//из полученного списка ранее ищем еще те которые подходят нам по модели
				accessories.Add(db.Accessories.FirstOrDefault(tr => tr.Id == access.Id));
			}

			var accesoriesName = new Dictionary<string, int>();
			foreach (var acc in accessories)
			{
				if (accesoriesName.ContainsKey(acc.Name))
				{
					accesoriesName[acc.Name]++;
				}
				else
				{
					accesoriesName.Add(acc.Name, 1);
				}
			}

			string FreeAccessoriesTitle = "";
			foreach (var fa in accesoriesName)
			{
				FreeAccessoriesTitle += fa.Key + " - " + fa.Value + "\n";
			}

			ViewData["FreeAccessoriesTitle"] = FreeAccessoriesTitle;


			ViewData["TransportModels"] = db.TransportModels.ToList();
			ViewData["FreeTransport"] = FreeTransport.Count;
			ViewData["FreeAccessories"] = FreeAccessories.Count;


			var ordersYesterday = db.Orders.Include(p => p.Payment).Include(t => t.OrderTransports.Select(r => r.Rates)).Where(o => o.DateStart >= dateYesterday).ToList();
			foreach (var order in ordersYesterday)
			{
				if (order.OrderTransports != null)
				{
					//Добавляем на всякий случай еще час
					var dateEndDuration = DateTime.Today.AddHours(order.OrderTransports.First().Rates.Duration + 1);
					if (order.DateEnd <= dateEndDuration)
					{
						var payment = order.Payment;
						YesterdayProfit += payment.CashDeposit + payment.CashPayment + payment.CardDeposit + payment.CardPayment;
						YesterdayCashPaymentProfit += payment.CashDeposit + payment.CashPayment;
						YesterdayCardPaymentProfit += payment.CardDeposit + payment.CardPayment;
						YesterdayBonusPaymentProfit += payment.BonusPayment;
					}
				}
			}
			ViewData["YesterdayProfit"] = YesterdayProfit;
			ViewData["YesterdayCashPaymentProfit"] = YesterdayCashPaymentProfit;
			ViewData["YesterdayCardPaymentProfit"] = YesterdayCardPaymentProfit;
			ViewData["YesterdayBonusPaymentProfit"] = YesterdayBonusPaymentProfit;



			var ordersToday = db.Orders.Include(p => p.Payment).Include(t => t.OrderTransports.Select(r => r.Rates)).Where(o => o.DateStart >= DateTime.Today).ToList();
			foreach (var order in ordersToday)
			{
				if (order.OrderTransports != null)
				{
					//Добавляем день и на всякий случай еще час
					var dateEndDuration = DateTime.Today.AddDays(1).AddHours(order.OrderTransports.First().Rates.Duration + 1);
					if (order.DateEnd <= dateEndDuration)
					{
						var payment = order.Payment;
						TodayProfit += payment.CashDeposit + payment.CashPayment + payment.CardDeposit + payment.CardPayment;
						TodayCashPaymentProfit += payment.CashDeposit + payment.CashPayment;
						TodayCardPaymentProfit += payment.CardDeposit + payment.CardPayment;
						TodayBonusPaymentProfit += payment.BonusPayment;
					}
				}
			}
			ViewData["TodayProfit"] = TodayProfit;
			ViewData["TodayCashPaymentProfit"] = TodayCashPaymentProfit;
			ViewData["TodayCardPaymentProfit"] = TodayCardPaymentProfit;
			ViewData["TodayBonusPaymentProfit"] = TodayBonusPaymentProfit;

			string CalendarFreeTransport = "<div class=\"d-flex bd-highlight\">	";
			CalendarFreeTransport += "<div class=\"d-flex flex-column bd-highlight mb-3 w-100\">";
			CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100\">Время/модель</div>";
			for (int i = 9; i <= 24; ++i)
			{
				CalendarFreeTransport += "<div class=\"p-1 bd-highlight border\">" + (i != 24 ? i.ToString() : "00") + ":00</div>";
			}
			CalendarFreeTransport += "</div>";


			var orders = db.OrderTransport.Include(o => o.Order).Include(tr => tr.Transport).ToList();

			foreach (var model in db.TransportModels)
			{
				CalendarFreeTransport += "<div class=\"d-flex flex-column bd-highlight mb-3 border w-100\">";

				CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100\">" + model.Name + "</div>";
				bool freeBackTime = false;
				for (int i = 9; i <= 24; ++i)
				{
					var timeSearch = DateTime.Today.AddHours(i);

					var transportFree = orders.Where(o => !(o.Order.DateStart >= timeSearch || o.Order.DateEnd <= timeSearch)).Where(t => t.Transport.TransportModels.Id == model.Id).ToList();

					if (transportFree.Count > 0)
					{
						CalendarFreeTransport += "<div class=\"p-1 bd-highlight border bg-info text-white text-center w-100\">" + (freeBackTime == false ? transportFree.Count.ToString() : "&nbsp;") + "</div>";
						if (!freeBackTime)
						{
							freeBackTime = true;
						}
					}
					else
					{
						freeBackTime = false;
						CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100 \">&nbsp;</div>";
					}
				}

				CalendarFreeTransport += "</div>";
			}
			CalendarFreeTransport += "</div>";
			ViewData["CalendarFreeTransport"] = CalendarFreeTransport;

			return View();
		}

		public ActionResult Filter(DateTime dateCalendar)
		{
			var trm = db.Transport.Include(tm => tm.TransportModels).ToList();

			string CalendarFreeTransport = "<div class=\"d-flex bd-highlight\">	";
			CalendarFreeTransport += "<div class=\"d-flex flex-column bd-highlight mb-3 w-100\">";
			CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100\">Время/модель</div>";
			for (int i = 9; i <= 24; ++i)
			{
				CalendarFreeTransport += "<div class=\"p-1 bd-highlight border\">" + (i != 24 ? i.ToString() : "00") + ":00</div>";
			}
			CalendarFreeTransport += "</div>";


			var orders = db.OrderTransport.Include(o => o.Order).Include(tr => tr.Transport).ToList();

			foreach (var model in db.TransportModels)
			{
				CalendarFreeTransport += "<div class=\"d-flex flex-column bd-highlight mb-3 border w-100\">";

				CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100\">" + model.Name + "</div>";
				bool freeBackTime = false;
				for (int i = 9; i <= 24; ++i)
				{
					var timeSearch = dateCalendar.AddHours(i);

					var transportFree = orders.Where(o => !(o.Order.DateStart >= timeSearch || o.Order.DateEnd <= timeSearch)).Where(t => t.Transport.TransportModels.Id == model.Id).ToList();

					if (transportFree.Count > 0)
					{
						CalendarFreeTransport += "<div class=\"p-1 bd-highlight border bg-info text-white text-center w-100\">" + (freeBackTime == false ? transportFree.Count.ToString() : "&nbsp;") + "</div>";
						if (!freeBackTime)
						{
							freeBackTime = true;
						}
					}
					else
					{
						freeBackTime = false;
						CalendarFreeTransport += "<div class=\"p-1 bd-highlight border w-100 \">&nbsp;</div>";
					}
				}

				CalendarFreeTransport += "</div>";
			}
			CalendarFreeTransport += "</div>";
			ViewData["CalendarFreeTransport"] = CalendarFreeTransport;

			return PartialView();
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}
		public ActionResult Order()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public void CreateOrderDocument()
		{
			var user = db.Users.Include(c => c.ContactUser).FirstOrDefault(u => u.Id == 1);
			string fileName = Server.MapPath(@"~\Data\Raport\Template\vehicle_lease_agreement.docx");

			var doc = DocX.Load(fileName);
			doc.ReplaceText("{USER_SHORT_NAME}", user.ContactUser.ShortName);
			var fileNameCreateRaport = Server.MapPath(@"~\Data\Raport\Generate\vehicle_lease_agreement" + DateTime.Now.Ticks.ToString() + ".docx");

			doc.SaveAs(fileNameCreateRaport);

			Process.Start("WINWORD.EXE", fileNameCreateRaport);
		}
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}