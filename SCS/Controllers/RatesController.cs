using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
//using System.Web.Mvc;
using SCS.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SCS.Controllers
{
	public class RatesController : Controller
	{
		private SCSContext db = new SCSContext();

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
			return View();
		}

		// POST: Rates/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,TimeStart,TimeEnd,Duration,Price,IsTransport")] Rates rates)
		{
			if (ModelState.IsValid)
			{	
				db.Rates.Add(rates);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

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
