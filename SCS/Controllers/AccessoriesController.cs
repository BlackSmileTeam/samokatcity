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
		private SCSContext db = new SCSContext();

		public List<SelectListItem> StatusAccessories { get; set; }
		public AccessoriesController()
		{
			StatusAccessories = new List<SelectListItem>();

			StatusAccessories.Add(new SelectListItem { Value="-1", Text = "Все" });

			var status = db.Helpers.Where(statusType => statusType.Code == 3).ToList();

			foreach (var tmpStatus in status)
			{
				StatusAccessories.Add(new SelectListItem { Value = tmpStatus.Value.ToString(), Text = tmpStatus.Text.ToString() });
			}
		}

		// GET: Accessories
		public async Task<ActionResult> Index()
		{
			ViewBag.StatusAccessories = StatusAccessories;
			ViewData["status"] = db.Helpers.Where(statusType=>statusType.Code == 3).ToList();
			return View(await db.Accessories.ToListAsync());
		}

		/// <summary>
		/// Фильтрация 
		/// </summary>
		/// <param name="status"></param>
		/// <param name="dateStart"></param>
		/// <param name="dateEnd"></param>
		/// <returns></returns>
		public ActionResult Filter(FormCollection collection)
		{
			List<Accessories> accessories = new List<Accessories>();

			if (collection[1] == "-1")
			{
				accessories = db.Accessories.ToList();
			}
			else
			{
				var idStatus = Convert.ToInt32(collection[1]);
				accessories = db.Accessories.Where(tr => tr.Status == idStatus).ToList();
			}
			ViewBag.StatusAccessories = StatusAccessories;
			ViewData["status"] = db.Helpers.Where(statusType => statusType.Code == 3).ToList();

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
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,Price")] Accessories accessories)
		{
			if (ModelState.IsValid)
			{
				accessories.Status = 1;
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
			var status = StatusAccessories;
			//Удаляем 0 элемент "Все"
			status.RemoveAt(0);
			foreach (var stat in status)
			{
				if (stat.Value == accessories.Status.ToString())
				{
					stat.Selected = true;
					break;
				}
			}
			ViewData["Status"] = status;
			return View(accessories);
		}

		// POST: Accessories/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status,Price")] Accessories accessories)
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
