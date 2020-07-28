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
    public class OrderAccessoriesController : Controller
    {
        private SCSContext db = new SCSContext();

        // GET: OrderAccessories
        public async Task<ActionResult> Index()
        {
            var orderAccessories = db.OrderAccessories.Include(o => o.Accessories).Include(o => o.Order).Include(o => o.Rates);
            return View(await orderAccessories.ToListAsync());
        }

        // GET: OrderAccessories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderAccessories orderAccessories = await db.OrderAccessories.FindAsync(id);
            if (orderAccessories == null)
            {
                return HttpNotFound();
            }
            return View(orderAccessories);
        }

        // GET: OrderAccessories/Create
        public ActionResult Create()
        {
            ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name");
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "StatusOrder");
            ViewBag.RatesId = new SelectList(db.Rates, "Id", "Name");

            return View();
        }

        // POST: OrderAccessories/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrderId,AccessoriesId,RatesId")] OrderAccessories orderAccessories)
        {
            if (ModelState.IsValid)
            {
                db.OrderAccessories.Add(orderAccessories);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", orderAccessories.AccessoriesId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "StatusOrder", orderAccessories.OrderId);
            ViewBag.RatesId = new SelectList(db.Rates, "Id", "Name", orderAccessories.RatesId);
            return View(orderAccessories);
        }

        // GET: OrderAccessories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderAccessories orderAccessories = await db.OrderAccessories.FindAsync(id);
            if (orderAccessories == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", orderAccessories.AccessoriesId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "StatusOrder", orderAccessories.OrderId);
            ViewBag.RatesId = new SelectList(db.Rates, "Id", "Name", orderAccessories.RatesId);
            return View(orderAccessories);
        }

        // POST: OrderAccessories/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrderId,AccessoriesId,RatesId")] OrderAccessories orderAccessories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderAccessories).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AccessoriesId = new SelectList(db.Accessories, "Id", "Name", orderAccessories.AccessoriesId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "StatusOrder", orderAccessories.OrderId);
            ViewBag.RatesId = new SelectList(db.Rates, "Id", "Name", orderAccessories.RatesId);
            return View(orderAccessories);
        }

        // GET: OrderAccessories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderAccessories orderAccessories = await db.OrderAccessories.FindAsync(id);
            if (orderAccessories == null)
            {
                return HttpNotFound();
            }
            return View(orderAccessories);
        }

        // POST: OrderAccessories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OrderAccessories orderAccessories = await db.OrderAccessories.FindAsync(id);
            db.OrderAccessories.Remove(orderAccessories);
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
