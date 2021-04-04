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
	public class PromotionsController : Controller
	{
		private SCSContext db = new SCSContext();
		// GET: Promotions
		public ActionResult Index()
		{

			var prom = from fac in db.Promotions
					   join sem in db.PromotionsTransportModels on fac.PromotionsTransportModels equals sem.Id;

			//select new { Faculty = fac, SemesterText = sem.SemesterText };



			//var ptm = db.PromotionsTransportModels.Include(m => m.TransportModels);
			var Promotions = db.Promotions.Include(ptm => ptm.PromotionsTransportModels.
			Join(db.TransportModels,
				p => p.TransportModels.Id,
				c => c.Id,
				(p, c) =>
				{
					c.Name = p.TransportModels.Name
				}));
			ViewData["TransportModels"] = db.TransportModels;
			return View(Promotions.ToList());
		}

		// GET: Promotions/Details/5
		public ActionResult Details(int id)
		{
			var Promotions = db.Promotions.Include(ptm => ptm.PromotionsTransportModels).FirstOrDefault(p => p.Id == id);

			return View();
		}

		// GET: Promotions/Create
		public ActionResult Create()
		{
			List<SelectListItem> dropdownItems = new List<SelectListItem>();
			dropdownItems.AddRange(new[]{
										new SelectListItem { Text = "Понедельник", Value = "1"},
										new SelectListItem { Text = "Вторник", Value = "2"},
										new SelectListItem { Text = "Среда", Value = "3"},
										new SelectListItem { Text = "Четверг", Value = "4"},
										new SelectListItem { Text = "Пятница", Value = "5"},
										new SelectListItem { Text = "Суббота", Value = "6"},
										new SelectListItem { Text = "Воскресенье", Value = "7"}
			});
			ViewData.Add("DayOfWeek", dropdownItems);

			return View();
		}

		// POST: Promotions/Create
		[HttpPost]
		//public ActionResult Create(string Name, string Description, int DayOfWeek,TimeSpan TimeStart, TimeSpan TimeEnd, decimal Discount, List<TransportModels> TransportSelect2)
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				int dayOfWeek = 0;
				int.TryParse(collection["DayOfWeek"].ToString(), out dayOfWeek);
				Promotions promotions = new Promotions()
				{
					Name = collection["Name"],
					Description = collection["Description"],
					DayOfWeek = dayOfWeek,
					TimeStart = TimeSpan.Parse(collection["TimeStart"]),
					TimeEnd = TimeSpan.Parse(collection["TimeEnd"]),
					Discount = decimal.Parse(collection["Discount"])
				};
				db.Promotions.Add(promotions);
				db.SaveChanges();

				var Models = collection["TransportSelect2"].Split(',');
				foreach (var model in Models)
				{
					db.PromotionsTransportModels.Add(new PromotionsTransportModels
					{
						TransportModels = db.TransportModels.Find(Convert.ToInt32(model)),
						Promotions = promotions
					});
				}
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View();
			}
		}

		// GET: Promotions/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Promotions/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Promotions/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Promotions/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
