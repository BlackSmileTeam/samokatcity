using SCS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCS.Controllers
{
	public class TModelsController : Controller
	{
		private SCSContext db = new SCSContext();
		// GET: Models
		public ActionResult Index()
		{
			return View(db.TransportModels.ToList());
		}

		// GET: Models/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Models/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Models/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				TransportModels transportModels = new TransportModels()
				{
					Name = collection["Name"],
					ChargingTime = decimal.Parse(collection["ChargingTime"]),
					Markup = decimal.Parse(collection["Markup"])
				};
				db.TransportModels.Add(transportModels);
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View();
			}
		}

		// GET: Models/Edit/5
		public ActionResult Edit(int id)
		{
			var transpoortModels = db.TransportModels.Find(id);

			if (transpoortModels == null)
			{
				return HttpNotFound();
			}

			return View(transpoortModels);
		}

		// POST: Models/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				var transportModels = db.TransportModels.Find(id);
				transportModels.Name = collection["Name"];
				transportModels.Markup = decimal.Parse(collection["Markup"].ToString().Replace('.', ','));
				transportModels.ChargingTime = decimal.Parse(collection["ChargingTime"].ToString().Replace('.', ','));

				db.Entry(transportModels).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View();
			}
		}

		// GET: Models/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TransportModels transportModels = db.TransportModels.Find(id);
			if (transportModels == null)
			{
				return HttpNotFound();
			}
			return View(transportModels);
		}

		// POST: Models/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				var TModel = db.TransportModels.Find(id);
				if (TModel == null)
				{
					return HttpNotFound();
				}

				var promotions = db.PromotionsTransportModels.Include(m => m.TransportModels).Where(tm => tm.TransportModels.Id == id);
				var transport = db.Transport.Include(m => m.TransportModels).Where(tm => tm.TransportModels.Id == id);

				foreach (var pr in promotions)
				{
					db.PromotionsTransportModels.Remove(pr);
				}
				db.SaveChanges();

				foreach (var tr in transport)
				{
					db.Transport.Remove(tr);
				}
				db.SaveChanges();

				db.TransportModels.Remove(TModel);

				db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
		}
	}
}
