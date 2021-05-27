using SCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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


			ViewData["TransportModels"] = db.TransportModels.ToList();
			ViewData["FreeTransport"] = db.Transport.SqlQuery("CALL transport_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList().Count;
			ViewData["FreeAccessories"] = db.Accessories.SqlQuery("CALL accessories_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList().Count;
			var users = db.Users;
			ViewBag.User = users;

			var dateYesterday = DateTime.Today.AddDays(-1);

			var orders = db.Orders.Include(p => p.Payment).Include(t => t.OrderTransports.Select(r => r.Rates)).Where(o => o.DateStart >= dateYesterday).ToList();
			foreach (var order in orders)
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

			return View();
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
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}