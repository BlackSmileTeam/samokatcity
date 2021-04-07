using SCS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SCS.Controllers
{
	public class PromotionsController : Controller
	{
		private SCSContext db = new SCSContext();
		// GET: Promotions
		public ActionResult Index()
		{
			return View(db.Promotions.Include(ptm => ptm.PromotionsTransportModels).ToList());
		}

		// GET: Promotions/Details/5
		public ActionResult Details(int id)
		{
			var promotions = db.Promotions.Include(ptm => ptm.PromotionsTransportModels.Select(m => m.TransportModels)).FirstOrDefault(pr=>pr.Id == id);

			if (promotions == null)
			{
				return HttpNotFound();
			}
			return PartialView(promotions);

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
			var promotions = db.Promotions.Include(ptm => ptm.PromotionsTransportModels.Select(m => m.TransportModels)).FirstOrDefault(pr => pr.Id == id);
			if (promotions == null)
			{
				return HttpNotFound();
			}

			return View(promotions);
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
