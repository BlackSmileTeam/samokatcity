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
	public class TransportsController : Controller
	{
		private SCSContext db = new SCSContext();

		public List<SelectListItem> StatusTransport { get; set; }
		public TransportsController()
		{
			StatusTransport = new List<SelectListItem>();

			StatusTransport.Add(new SelectListItem { Text = "Все" });

			var ListStatus = db.Helpers.Where(h => h.Code == 2).ToList();
			foreach (var tmpStatus in ListStatus)
			{
				StatusTransport.Add(new SelectListItem { Text = tmpStatus.Text });
			}
		}

		/// <summary>
		/// Фильтрация 
		/// </summary>
		/// <param name="status"></param>
		/// <param name="dateStart"></param>
		/// <param name="dateEnd"></param>
		/// <returns></returns>
		public ActionResult Filter(string StatusTransport)
		{
			List<Transport> transports = new List<Transport>();
			
			if (StatusTransport == "Все")
			{
				transports = db.Transport.Include(tr => tr.OrderTransports).ToList();
			}
			else
			{
				transports = db.Transport.Include(tr => tr.OrderTransports).Where(h => h.Status.Text == StatusTransport && h.Status.Code == 2).ToList();

			}
			ViewBag.StatusOrder = StatusTransport;

			return PartialView(transports);
		}

		// GET: Transports
		public async Task<ActionResult> Index()
		{
			ViewBag.StatusTransport = StatusTransport;
			return View(await db.Transport.Include(tr => tr.OrderTransports).Include(m=>m.TransportModels).ToListAsync());
		}

		// GET: Transports/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Transports/Create
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Name,Status, Markup")] Transport transport)
		{
			if (ModelState.IsValid)
			{
				transport.Status.Value = 1;
				db.Transport.Add(transport);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(transport);
		}

		// GET: Transports/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transport transport = await db.Transport.Include(h => h.Status.Code == 2).FirstOrDefaultAsync(t => t.Id == id);
			if (transport == null)
			{
				return HttpNotFound();
			}

			SelectList status = new SelectList(db.Helpers.Where(h => h.Code == 2).ToList(), "Key", "Value", transport.Status.Value);
			ViewBag.StatusTransport = status;

			return View(transport);
		}

		// POST: Transports/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status,Markup")] Transport transport)
		{
			if (ModelState.IsValid)
			{
				db.Entry(transport).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(transport);
		}

		// GET: Transports/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transport transport = await db.Transport.FindAsync(id);
			if (transport == null)
			{
				return HttpNotFound();
			}
			return View(transport);
		}

		// POST: Transports/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Transport transport = await db.Transport.FindAsync(id);
			db.Transport.Remove(transport);
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
