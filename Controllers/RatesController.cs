using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCS.Models;
using System.Web.UI.WebControls;

namespace SCS.Controllers
{
	public class RatesController : Controller
	{
		private SCSContext db = new SCSContext();

		// GET: Rates
		public async Task<ActionResult> Index()
		{
			var rates = db.Rates.Include(r => r.Accessories).Include(r => r.Transport);
			bool checkAccessories = false;
			bool checkTransport = false;
			//Проверяем наличие тарифов для аксуссуаров и транспорта, если есть записи для обоих видов, то выходим из проверки
			foreach (var rat in rates)
			{
				if (rat.TransportId != null)
				{
					checkTransport = true;
				}
				if (rat.AccessoriesId != null)
				{
					checkAccessories = true;
				}
				if (checkAccessories == true && checkTransport == true)
				{
					break;
				}
			}

			ViewBag.CheckAccessories = checkAccessories;
			ViewBag.CheckTransport = checkTransport;

			return View(await rates.ToListAsync());
		}

		// GET: Rates/Details/5
		public async Task<ActionResult> Details(int? id)
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

		// GET: Rates/Create
		public ActionResult Create()
		{
			SelectList accessoriesList = new SelectList(db.Accessories, "Id", "Name");
			accessoriesList.Append(new SelectListItem() { Text = "Отсутствует", Value = "0" });
			ViewBag.AccessoriesId = accessoriesList;
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name");
			return View();
		}

		// POST: Rates/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,TimeStart,TimeEnd,Duration,Price,TransportId,AccessoriesId")] Rates rates,bool showAccessories, bool showTransport)
		{
			if (ModelState.IsValid)
			{
				if (showTransport == false)
				{
					rates.TransportId = null;
					rates.Transport = null;
				}
				if (showAccessories == false)
				{
					rates.AccessoriesId = null;
					rates.Accessories = null;
				}
				db.Rates.Add(rates);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", rates.AccessoriesId);
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name", rates.TransportId);
			return View(rates);
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
			if (rates.TransportId != null)
			{
				ViewBag.Transport = true;
			}
			else
			{
				ViewBag.Transport = false;
			}
			ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", rates.AccessoriesId);
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name", rates.TransportId);
			return View(rates);
		}

		// POST: Rates/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TimeStart,TimeEnd,Duration,Price,TransportId,AccessoriesId")] Rates rates)
		{
			if (ModelState.IsValid)
			{
				db.Entry(rates).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", rates.AccessoriesId);
			ViewBag.TransportId = new SelectList(db.Transport, "Id", "Name", rates.TransportId);
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

		// POST: Rates/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Rates rates = await db.Rates.FindAsync(id);
			db.Rates.Remove(rates);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
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
