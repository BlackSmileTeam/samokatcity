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
    public class UsersController : Controller
    {
        private SCSContext db = new SCSContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.ContactUser);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Include(cu => cu.ContactUser).FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //user.ContactUser = db.ContactUser.Find(user.ContactUserId);


            return PartialView(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string Username, string Password, string Passport, string City,
                                               string Home, string Apartament, string Surname, string Name, string Street,
                                               string Patronymic, string Phone)
        {
            User user = new User();
            if (ModelState.IsValid)
            {
                user = new User()
                {
                    Username = Username,
                    Password = Password
                };
                var userInitialsName = (Name != null && Name.Length > 0) ? Name[0] + "." : " ";
                var userInitialsPatronymic = (Patronymic != null && Patronymic.Length > 0) ? Patronymic[0] + "." : "";
                СontactUser contactUser = new СontactUser()
                {
                    Passport = Passport,
                    City = City,
                    Home = Home,
                    Apartment = Apartament,
                    Surname = Surname,
                    Name = Name,
                    Patronymic = Patronymic,
                    Phone = Phone,
                    Street = Street,
                    ShortName = $"{Surname} {userInitialsName}  {userInitialsPatronymic}"
                };
                db.ContactUser.Add(contactUser);
                await db.SaveChangesAsync();

                //Добавляем созданные данные пользователя к авторизационным данным
                user.ContactUser = contactUser;
                //У полученные данных пользователя и сохраненных в БД берем ID (которые сгенерирован автоматически) и присваиваем в модель пользователя
                //user.ContactUserId = contactUser.Id;

                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewBag.ContactUserId = new SelectList(db.ContactUser, "Id", "ShortName", user.ContactUserId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Include(c => c.ContactUser).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int Id, string Username, string Password, string Passport, string City,
                                               string Home, string Apartment, string Surname, string Street,
                                               string Patronymic, string Phone, string Name)
        {
            User user = db.Users.Include(cu => cu.ContactUser).FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                user.Username = Username;
                user.Password = Password;
                var userInitialsName = (Name != null && Name.Length > 0) ? Name[0] : ' ';
                var userInitialsPatronymic = (Patronymic != null && Patronymic.Length > 0) ? Patronymic[0] : ' ';


                user.ContactUser.Passport = Passport;
                user.ContactUser.City = City;
                user.ContactUser.Home = Home;
                user.ContactUser.Apartment = Apartment;
                user.ContactUser.Surname = Surname;
                user.ContactUser.Name = Name;
                user.ContactUser.Patronymic = Patronymic;
                user.ContactUser.Phone = Phone;
                user.ContactUser.Street = Street;
                user.ContactUser.ShortName = $"{Surname} {userInitialsName}. {userInitialsPatronymic}.";

                if (ModelState.IsValid)
                {

                    db.Entry(user).State = EntityState.Modified;
                    //db.Entry(сontactUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Include(cu => cu.ContactUser).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);

            var orders = db.Orders.Include(u => u.User).Include(p => p.Payment).Where(o => o.User.Id == id);

            List<OrderAccessories> orderAccessories = db.OrderAccessories.Include(o => o.Order).ToList();
            List<OrderTransport> orderTransports = db.OrderTransport.Include(o => o.Order).ToList();
            foreach (var order in orders)
            {
                orderAccessories = orderAccessories.Where(oa => oa.Order.Id == order.Id).ToList();
                orderTransports = orderTransports.Where(oa => oa.Order.Id == order.Id).ToList();

                foreach (var oa in orderAccessories)
                {
                    db.OrderAccessories.Remove(oa);
                }
                foreach (var ot in orderTransports)
                {
                    db.OrderTransport.Remove(ot);
                }

                db.Orders.Remove(order);

                if (order.Payment != null)
                {
                    user.Bonus -= order.AddBonuses + order.Payment.BonusPayment;
                    db.Entry(user).State = EntityState.Modified;
                    db.Payment.Remove(order.Payment);
                }
            }

            await db.SaveChangesAsync();

            db.Users.Remove(user);

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
