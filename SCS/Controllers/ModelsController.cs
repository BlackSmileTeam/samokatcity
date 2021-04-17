using SCS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SCS.Controllers
{
	public class ModelsController : Controller
	{
		private SCSContext db = new SCSContext();
		// GET: Models
		public async Task<ActionResult> Index()
		{
			return View(await db.TransportModels.ToListAsync());
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
			return View();
		}

		// POST: Models/Edit/5
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

		// GET: Models/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Models/Delete/5
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
