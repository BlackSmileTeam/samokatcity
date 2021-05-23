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
			var promotions = db.Promotions.Include(ptm => ptm.PromotionsTransportModels.Select(m => m.TransportModels)).FirstOrDefault(pr => pr.Id == id);

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
				//int.TryParse(collection["DayOfWeek"].ToString(), out dayOfWeek);
				Promotions promotions = new Promotions()
				{
					Name = collection["Name"],
					Description = collection["Description"],
					DayOfWeek = collection["DayOfWeek"],
					TimeStart = TimeSpan.Parse(collection["TimeStart"]),
					TimeEnd = TimeSpan.Parse(collection["TimeEnd"]),
					Discount = decimal.Parse(collection["Discount"].ToString().Replace('.', ','))
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
			
			var promotions = db.Promotions.Include(trm => trm.PromotionsTransportModels.Select(m => m.TransportModels)).FirstOrDefault(p => p.Id == id);

			if (promotions == null)
			{
				return HttpNotFound();
			}

			var selectDay = promotions.DayOfWeek.Split(',');
			foreach (var day in selectDay)
			{				
				dropdownItems[int.Parse(day)].Selected = true;
			}
			ViewData.Add("DayOfWeek", dropdownItems);
			return View(promotions);
		}

		// POST: Promotions/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				int dayOfWeek = 0;
				int.TryParse(collection["DayOfWeek"].ToString(), out dayOfWeek);

				var promotions = db.Promotions.Find(id);
				promotions.Name = collection["Name"];
				promotions.Description = collection["Description"];
				promotions.DayOfWeek = collection["DayOfWeek"];
				promotions.TimeStart = TimeSpan.Parse(collection["TimeStart"]);
				promotions.TimeEnd = TimeSpan.Parse(collection["TimeEnd"]);
				promotions.Discount = decimal.Parse(collection["Discount"].ToString().Replace('.', ','));


				db.Entry(promotions).State = EntityState.Modified;
				db.SaveChanges();

				var Models = collection["TransportSelect2"].Split(',');

				var ptm = db.PromotionsTransportModels.Include(p => p.Promotions).Where(p => p.Promotions.Id == id).ToList();
				foreach (var PTM in ptm)
				{
					db.PromotionsTransportModels.Remove(PTM);
				}

				db.SaveChanges();

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
				var promot = db.Promotions.Find(id);
				var ptm = db.PromotionsTransportModels.Include(p => p.Promotions).Where(p => p.Promotions.Id == id).ToList();
				if (promot == null && ptm == null)
				{
					return HttpNotFound();
				}

				foreach (var PTM in ptm)
				{
					db.PromotionsTransportModels.Remove(PTM);
				}

				db.Promotions.Remove(promot);

				db.SaveChanges();
			}
			catch
			{
			}

			return RedirectToAction("Index");
		}
	}
}
