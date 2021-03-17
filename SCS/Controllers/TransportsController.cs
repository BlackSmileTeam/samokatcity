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

		private Dictionary<int, string> statusTransport = new Dictionary<int, string>
		{
			{
				0,"Отсутствует"
			},
			{
				1,"В наличии"
			},
			{
				2 ,"На зарядке"
			}
		};
		public List<SelectListItem> StatusTransport { get; set; }
		public TransportsController()
		{
			StatusTransport = new List<SelectListItem>();

			StatusTransport.Add(new SelectListItem { Text = "Все" });

			foreach (string tmpStatus in statusTransport.Values)
			{
				StatusTransport.Add(new SelectListItem { Text = tmpStatus });
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
			int str = 0;
			switch (StatusTransport)
			{
				case "В наличии":
					{
						str = 1;
						break;
					}
				case "Отсутствует":
					{
						str = 0;
						break;
					}
				case "На зарядке":
					{
						str = 2;
						break;
					}
				default:
					{
						str = -1;
						break;
					}
			}
			if (str == -1)
			{
				transports = db.Transport.Include(tr => tr.OrderTransports).ToList();
			}
			else
			{
				transports = db.Transport.Include(tr => tr.OrderTransports).Where(tr => tr.Status == str).ToList();

			}
			ViewBag.StatusOrder = StatusTransport;

			return PartialView(transports);
		}

		// GET: Transports
		public async Task<ActionResult> Index()
		{
			ViewBag.StatusTransport = StatusTransport;
			return View(await db.Transport.Include(tr => tr.OrderTransports).ToListAsync());
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

			SelectList status = new SelectList(statusTransport, "Key", "Value", transport.Status);
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
