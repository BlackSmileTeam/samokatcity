﻿using System.Linq;
using System.Web.Http;
using SCS.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace SCS.Controllers.API
{
	public class WebController : ApiController
	{
		private SCSContext db = new SCSContext();
		// GET: api/Web
		//public IEnumerable<string> Get()
		//{
		//	return new string[] { "valsue1", "value2" };
		//}
		public string Get()
		{
			return "Welcome To Web API";
		}
		/// <summary>
		/// Поиск клиента по части строки
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
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
		/// Запрос на получение количества бонусов у пользователя
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public decimal SearchUserBonus(string id)
		{
			decimal idUser = Convert.ToDecimal(id);
			decimal bonus = 0;
			if (idUser > 0)
			{
				bonus = db.Users.FirstOrDefault(u => u.Id == idUser).Bonus;
			}
			return bonus;
		}

		/// <summary>
		/// Получаем значение наценки для выбранного ТС
		/// </summary>
		/// <param name="nameTransport"></param>
		/// <returns></returns>
		[HttpGet]
		public decimal ValueMarkupTransport(string id)
		{
			decimal markup = 0;
			if (id != null && id.Length > 0 && id != "0")
			{
				int idTransport = Convert.ToInt32(id);
				if (DateTime.Now.DayOfWeek.ToString().ToLower() == "суббота" || DateTime.Now.DayOfWeek.ToString().ToLower() == "воскресенье" ||
					DateTime.Now.DayOfWeek.ToString().ToLower() == "saturday" || DateTime.Now.DayOfWeek.ToString().ToLower() == "sunday")
				{
					markup = db.Transport.Where(tr => tr.Id == idTransport).ToList()[0].Markup;
				}
			}

			return markup;
		}
		[HttpGet]
		public decimal TarifPrice(string id)
		{
			decimal price = 0;
			if (id != null && id.Length != 0)
			{
				int idRates = Convert.ToInt32(id);
				price = db.Rates.Where(tr => tr.Id == idRates).ToList()[0].Price;
			}
			return price;
		}		
	}
}