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
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = await db.Users.FindAsync(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			user.ContactUser = db.ContactUser.Find(user.ContactUserId);


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
											   string Home, string Apartament, string Surname, string Name,
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
					ShortName = $"{Surname} {Name[0]}. {Patronymic[0]}."
				};
				db.ContactUser.Add(contactUser);
				await db.SaveChangesAsync();

				//Добавляем созданные данные пользователя к авторизационным данным
				user.ContactUser = contactUser;
				//У полученные данных пользователя и сохраненных в БД берем ID (которые сгенерирован автоматически) и присваиваем в модель пользователя
				user.ContactUserId = contactUser.Id;

				db.Users.Add(user);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.ContactUserId = new SelectList(db.ContactUser, "Id", "ShortName", user.ContactUserId);
			return View(user);
		}

		// GET: Users/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = await db.Users.FindAsync(id);
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
											   string Home, string Apartament, string Surname, string Name,
											   string Patronymic, string Phone)
		{
			User user = db.Users.Find(Id);
			user.Username = Username;
			user.Password = Password;
			СontactUser сontactUser = db.ContactUser.Find(user.ContactUserId);
			сontactUser.Passport = Passport;
			сontactUser.City = City;
			сontactUser.Home = Home;
			сontactUser.Apartment = Apartament;
			сontactUser.Surname = Surname;
			сontactUser.Name = Name;
			сontactUser.Patronymic = Patronymic;
			сontactUser.Phone = Phone;
			сontactUser.ShortName = $"{Surname} {Name[0]}. {Patronymic}.";

			if (ModelState.IsValid)
			{

				db.Entry(user).State = EntityState.Modified;
				db.Entry(сontactUser).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
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
			User user = await db.Users.FindAsync(id);
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
			СontactUser сontactUser = await db.ContactUser.FindAsync(user.ContactUserId);
			db.Users.Remove(user);
			db.ContactUser.Remove(сontactUser);
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
