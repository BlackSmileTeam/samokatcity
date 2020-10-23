using System.Linq;
using System.Web.Http;
using SCS.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;
//using System.Web.Mvc;
using System;

namespace SCS.Controllers.API
{
	public class WebController : ApiController
	{
		// GET: api/Web
		public IEnumerable<string> Get()
		{
			return new string[] { "valsue1", "value2" };
		}

		[HttpGet]
		public async Task<List<User>> GetAsync(string term)
		{
			if (!string.IsNullOrEmpty(term))
			{
				string json = "";

				var states = await db.Users.Include(u => u.ContactUser).ToListAsync();
				var data = states.Where(a => a.ContactUser.Name.Contains(term)
				|| a.ContactUser.Surname.Contains(term)
				|| a.ContactUser.Patronymic.Contains(term)).ToList();

				json = JsonConvert.SerializeObject(data, Formatting.Indented);

				return data;

			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// _Запрос на получение количества бонусов у пользователя
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public decimal searchUserBonus(string id)
		{
			decimal idUser = Convert.ToDecimal(id);
			decimal bonus = 0;
			if (idUser > 0)
			{
				bonus = db.Users.FirstOrDefault(u => u.Id == idUser).Bonus;
			}
			return bonus;
		}


		//public decimal Post([System.Web.Http.FromBody] string idUser)
		//{
		//	decimal bonus = 0;
		//	if (idUser > 0)
		//	{
		//		bonus = db.Users.FirstOrDefault(u => u.Id == idUser).Bonus;
		//	}
		//	return bonus;
		//}

		// POST: api/Web
		//public void Post([System.Web.Http.FromBody] string value)
		//{
		//}

		// PUT: api/Web/5
		public void Put(int id, [System.Web.Http.FromBody] string value)
		{
		}

		// DELETE: api/Web/5
		public void Delete(int id)
		{
		}
		private SCSContext db = new SCSContext();

	}
}
