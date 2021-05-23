using System.Data.Entity;
using System.Threading.Tasks;
using SCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace SCS.Controllers
{
	public class RatesController : Controller
	{
		private SCSContext db = new SCSContext();
		private int countModels
		{
			get
			{
				if (Session["countModels"] != null)
				{
					return Convert.ToInt32(Session["countModels"]);
				}
				return 0;
			}
			set
			{
				Session["countModels"] = value;
			}
		}

		// GET: Rates
		public async Task<ActionResult> Index()
		{
			var rates = db.Rates;

			return View(await rates.ToListAsync());
		}

		// GET: Rates/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var rates = db.Rates.Include(rt => rt.RatesTransports.Select(m => m.TransportModels)).FirstOrDefault(pr => pr.Id == id);
			if (rates == null)
			{
				return HttpNotFound();
			}
			return PartialView(rates);
		}

		// GET: Rates/Create
		public ActionResult Create()
		{
			countModels = 0;
			return View();
		}

		// POST: Rates/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				Rates rates = new Rates()
				{
					Name = collection["Name"],
					Description = collection["Description"]
				};
				if (int.TryParse(collection["Duration"], out int duration))
				{
					rates.Duration = duration;
				}

				db.Rates.Add(rates);

				var models = collection["Model"].Split(',');
				var prices = collection["Price"].Split(',');
				for (int i = 0, countModels = models.Count(); i < countModels; ++i)
				{
					if (int.TryParse(models[i], out int numberModel))
					{
						RatesTransports ratesTransports = new RatesTransports()
						{
							TransportModels = db.TransportModels.Find(numberModel),
							Rates = rates
						};
						if (decimal.TryParse(prices[i], out decimal price))
						{
							ratesTransports.Price = price;
						}
						db.RatesTransports.Add(ratesTransports);
					}
				}
			}
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		// GET: Rates/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Rates rates = await db.Rates.FindAsync(id);
			if (rates == null)
			{
				return HttpNotFound();
			}
			return View(rates);
		}

		// POST: Rates/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TimeStart,TimeEnd,Duration,Price,IsTransport")] Rates rates)
		{
			if (ModelState.IsValid)
			{
				db.Entry(rates).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(rates);
		}

		// GET: Rates/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Rates rates = await db.Rates.FindAsync(id);
			if (rates == null)
			{
				return HttpNotFound();
			}
			return View(rates);
		}


		// POST: Promotions/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				var rates = db.Rates.Find(id);
				var ratesTransport = db.RatesTransports.Include(r => r.Rates).Where(r => r.Rates.Id == id);
				foreach (var rTransport in ratesTransport)
				{
					db.RatesTransports.Remove(rTransport);
				}
				db.Rates.Remove(rates);
				db.SaveChanges();
			}
			catch
			{
			}

			return RedirectToAction("Index");
		}
		public ActionResult AddDropListModels()
		{
			//Добавляем Id для добавленной модели
			ViewData["countModels"] = countModels;
			//Увеличиваем id на единицу, что бы следующий блок был с новым id
			++countModels;
			Session["countModels"] = countModels;
			SelectList Models = new SelectList(db.TransportModels, "Id", "Name");
			ViewBag.Models = Models;
			return View(db.TransportModels);
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
