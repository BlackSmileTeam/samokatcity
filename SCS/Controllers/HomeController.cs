using SCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCS.Controllers
{
	public class HomeController : Controller
	{
		SCSContext db = new SCSContext();
		public ActionResult Index()
		{
			ViewData["TransportModels"] = db.TransportModels.ToList();
			ViewData["FreeTransport"] = db.Transport.SqlQuery("CALL transport_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList().Count;
			ViewData["FreeAccessories"] = db.Accessories.SqlQuery("CALL accessories_vw('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')").ToList().Count;
			var users = db.Users;
			ViewBag.User = users;

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