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

			StatusTransport.Add(new SelectListItem { Value = "-1", Text = "Все" });

			var status = db.Helpers.Where(statusType => statusType.Code == 2).ToList();
			foreach (var stTr in status)
			{
				StatusTransport.Add(new SelectListItem { Value = stTr.Value.ToString(), Text = stTr.Text });
			}
		}

		// GET: Transports
		public async Task<ActionResult> Index()
		{
			ViewBag.StatusTransport = StatusTransport;
			ViewData["status"] = db.Helpers.Where(statusType => statusType.Code == 2).ToList();
			return View(await db.Transport.Include(m => m.TransportModels).ToListAsync());
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

			ViewData["StatusTransport"] = this.StatusTransport;
			if (StatusTransport == "-1")
			{
				transports = db.Transport.Include(m => m.TransportModels).ToList();
			}
			else
			{
				var statusNumber = int.Parse(StatusTransport);
				transports = db.Transport.Include(m => m.TransportModels).Where(t => t.Status == statusNumber).ToList();
			}
			ViewBag.StatusOrder = StatusTransport;

			return PartialView(transports);
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
		public async Task<ActionResult> Create(FormCollection collection)
		{
			Transport transport = new Transport();
			if (ModelState.IsValid)
			{
				if (int.TryParse(collection["TransportSelect2"].ToString(), out int idModel))
				{
					transport.TransportModels = db.TransportModels.Find(idModel);
				}
				transport.SerialNumber = collection["SerialNumber"];
				transport.IndexNumber = collection["IndexNumber"];
				transport.IsBlocked = false;
				transport.Status = 1;
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
			Transport transport = await db.Transport.FindAsync(id);
			if (transport == null)
			{
				return HttpNotFound();
			}
			var status = StatusTransport;
			//Удаляем 0 элемент "Все"
			status.RemoveAt(0);
			foreach (var stat in status)
			{
				if (stat.Value == transport.Status.ToString())
				{
					stat.Selected = true;
					break;
				}
			}
			ViewData["Status"] = status;
			return View(transport);
		}

		// POST: Transports/Edit/5
		// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
		// Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, FormCollection collection)
		{
			Transport transport = new Transport();
			if (ModelState.IsValid)
			{
				transport = db.Transport.Find(id);
				transport.IndexNumber = collection["IndexNumber"];
				transport.SerialNumber = collection["SerialNumber"];
				transport.Status = int.Parse(collection["Status"]);
				TransportModels transportModels = new TransportModels();
				if (int.TryParse(collection["TransportSelect2"], out int idModel))
				{
					transportModels = db.TransportModels.Find(idModel);
				}

				transport.TransportModels = transportModels;

				db.Entry(transport).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(transport);
		}

		// GET: Transports/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Transport transport = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == id);
			if (transport == null)
			{
				return HttpNotFound();
			}
			return View(transport);
		}

		// POST: Transports/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				Transport transport = db.Transport.Find(id);
				db.Transport.Remove(transport);
				var orderTransport = db.OrderTransport.Include(tr => tr.Transport).Where(tr => tr.Transport.Id == id);

				foreach (var ot in orderTransport)
				{
					db.Orders.Remove(ot.Order);
					db.OrderTransport.Remove(ot);
				}

				db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
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
