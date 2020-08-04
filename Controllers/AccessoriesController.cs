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

namespace SCS.Controllers
{
	public class AccessoriesController : Controller
	{

		private List<string> statusAccessories = new List<string>
		{
			"В наличии", "Отсутствует"
		};
		public List<SelectListItem> StatusAccessories { get; set; }
		public AccessoriesController()
		{
			StatusAccessories = new List<SelectListItem>();

			foreach (string tmpStatus in statusAccessories)
			{
				StatusAccessories.Add(new SelectListItem { Text = tmpStatus });
			}
		}
		private SCSContext db = new SCSContext();

		// GET: Accessories
		public async Task<ActionResult> Index()
		{
			ViewBag.StatusAccessories = StatusAccessories;
			return View(await db.Accessories.Include(tr => tr.OrderAccessories).ToListAsync());
		}

		/// <summary>
		/// Фильтрация 
		/// </summary>
		/// <param name="status"></param>
		/// <param name="dateStart"></param>
		/// <param name="dateEnd"></param>
		/// <returns></returns>
		public ActionResult Filter(string StatusAccessories)
		{
			bool str = false;
			switch (StatusAccessories)
			{
				case "В наличии":
					{
						str = true;
						break;
					}
				case "Отсутствует":
					{
						str = false;
						break;
					}
			}
			var accessories = db.Accessories.Include(tr => tr.OrderAccessories).Where(tr => tr.Status == str).ToList();

			ViewBag.StatusOrder = StatusAccessories;

			return PartialView(accessories);
		}

		// GET: Accessories/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Accessories/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,Status")] Accessories accessories)
		{
			if (ModelState.IsValid)
			{
				accessories.Status = true;
				db.Accessories.Add(accessories);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(accessories);
		}

		// GET: Accessories/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Accessories accessories = await db.Accessories.FindAsync(id);
			if (accessories == null)
			{
				return HttpNotFound();
			}
			return View(accessories);
		}

		// POST: Accessories/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status")] Accessories accessories)
		{
			if (ModelState.IsValid)
			{
				db.Entry(accessories).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(accessories);
		}

		// GET: Accessories/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Accessories accessories = await db.Accessories.FindAsync(id);
			if (accessories == null)
			{
				return HttpNotFound();
			}
			return View(accessories);
		}

		// POST: Accessories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Accessories accessories = await db.Accessories.FindAsync(id);
			db.Accessories.Remove(accessories);
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
