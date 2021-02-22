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
			//using (SCSContext db = new SCSContext())
			//{
			//	var users = db.User;
			//	foreach (User u in users)
			//	{
			//		Console.WriteLine("{0}.{1} - {2}", u.Id, u.Username, u.Password);
			//	}
			//}
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