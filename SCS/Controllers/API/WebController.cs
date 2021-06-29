using System.Linq;
using System.Web.Http;
using SCS.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

using System.Web.SessionState;

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
                var states = await db.Users.Include(u => u.ContactUser).Where(a => a.ContactUser.Name.Contains(term)
                || a.ContactUser.Surname.Contains(term)
                || a.ContactUser.Patronymic.Contains(term)
                || a.ContactUser.Phone.Contains(term)).ToListAsync();

                return states;
            }
            else
            {
                return await db.Users.Include(u => u.ContactUser).Take(10).ToListAsync();
            }
        }
        [HttpGet]
        public List<TransportModels> GetModel(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var model = db.TransportModels.Where(m => m.Name.Contains(term)).ToList();
                return model;
            }
            else
            {
                return db.TransportModels.Take(10).ToList();
            }
        }
        public class ModelsSelect2
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool Select { get; set; }
        }
        [HttpGet]
        public async Task<List<ModelsSelect2>> GetModelPromotionsEdit(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                return null;
            }
            //else
            //{
            //	return db.TransportModels.Take(10).ToList();
            //}

            var formattedData = db.PromotionsTransportModels.Include(p => p.Promotions).Include(m => m.TransportModels).Where(ptm => ptm.Promotions.Id == Convert.ToInt32(term)).ToList();
            List<ModelsSelect2> ms2 = new List<ModelsSelect2>();
            foreach (var model in formattedData)
            {
                ms2.Add(new ModelsSelect2
                {
                    Name = model.TransportModels.Name,
                    Id = model.TransportModels.Id,
                    Select = true
                });

            }

            return ms2;
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
        public decimal ValueMarkupTransport(string id, DateTime dateStart)
        {
            decimal markup = 0;
            if (id != null && id.Length > 0 && id != "0")
            {
                int idTransport = Convert.ToInt32(id);
                int idTransportModel = 0;
                var transModel = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(t => t.Id == idTransport);
                if (transModel != null)
                {
                    idTransportModel = transModel.TransportModels.Id;

                    var promTr = db.PromotionsTransportModels.Include(tm => tm.TransportModels)
                                                             .Include(p => p.Promotions)
                                                             .Where(tm => tm.TransportModels.Id == idTransportModel &&
                                                                    tm.Promotions.TimeStart.Hours <= dateStart.Hour &&
                                                                    tm.Promotions.TimeEnd.Hours >= dateStart.Hour)
                                                             .ToList();
                    foreach (var prTr in promTr)
                    {
                        markup += prTr.Promotions.Discount;
                    }
                }
            }

            return markup;
        }
        [HttpGet]
        public decimal TarifPriceAccessories(string model, string countAccessories)
        {
            decimal price = 0;

            int countAc = Convert.ToInt32(countAccessories);
            if (countAc > 0)
            {
                if (!string.IsNullOrEmpty(model))
                {
                    int idModel = Convert.ToInt32(model);
                    price = db.Accessories.Find(idModel).Price;
                    price *= countAc;
                }
            }
            return price;
        }
        [HttpGet]
        public decimal TarifPrice(string model, string rate, string countTransport, DateTime dateStart, int promotionId)
        {
            decimal price = 0;

            int countTr = Convert.ToInt32(countTransport);
            if (countTr > 0)
            {
                if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(rate))
                {
                    int idModel = Convert.ToInt32(model);
                    int idRates = Convert.ToInt32(rate);
                    var transport = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(t => t.Id == idModel);
                    price = db.RatesTransports.Include(r => r.Rates).Include(t => t.TransportModels).FirstOrDefault(p => p.Rates.Id == idRates && p.TransportModels.Id == transport.TransportModels.Id).Price;
                    price *= countTr;

                    int idTransportModel = 0;
                    var transModel = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(t => t.Id == idModel);
                    if (transModel != null)
                    {
                        idTransportModel = transModel.TransportModels.Id;

                        int dayOfWeek = (int)dateStart.DayOfWeek;
                        if (promotionId != -1)
                        {
                            price -= db.Promotions.Find(promotionId).Discount;
                        }
                    }
                }
            }
            return price;
        }
        /// <summary>
        /// Проверка наличия тарифа в указанное время
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isTransport"></param>
        /// <returns></returns>
        [HttpGet]
        public bool CheckRate(DateTime dateTime, bool isTransport)
        {
            //var rates = db.Rates.Where(x => x.IsTransport == isTransport).Where(x => x.TimeStart <= dateTime.TimeOfDay && x.TimeEnd >= dateTime.TimeOfDay);
            //return rates.Count() > 0 ? true : false;
            return false;
        }
        [HttpGet]
        public bool CheckTransport(DateTime dateTime)
        {
            List<Transport> trans = db.Transport.SqlQuery("CALL transport_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").ToList();

            return trans.Count() > 0 ? true : false;
        }
        [HttpGet]
        public bool CheckAccessories(DateTime dateTime)
        {
            List<Accessories> access = db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").ToList();

            return access.Count() > 0 ? true : false;
        }
        [HttpGet]
        public int CountFreeAccessories(DateTime dateTime, string nameAccessories)
        {
            List<Accessories> access = new List<Accessories>();
            if (!string.IsNullOrEmpty(nameAccessories))
            {
                var nameAccessoriesFind = db.Accessories.Find(Convert.ToInt32(nameAccessories)).Name;
                access = db.Accessories.SqlQuery("CALL accessories_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").Where(ac => ac.Name == nameAccessoriesFind).ToList();
            }
            return access.Count();
        }
        [HttpGet]
        public int CountFreeTransport(DateTime dateTime, string idTransportModels)
        {
            List<Transport> transports = new List<Transport>();
            if (!string.IsNullOrEmpty(idTransportModels))
            {
                int idTrModel = Convert.ToInt32(idTransportModels);
                var nameTransportModelsFind = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == idTrModel).TransportModels.Id;

                var transp = db.Transport.SqlQuery("CALL transport_vw('" + dateTime.ToString("yyyy-MM-dd HH:mm") + "')").ToList();
                List<int> idTransp = new List<int>();

                foreach (var trans in transp)
                {
                    var tmpTransp = db.Transport.Include(tm => tm.TransportModels).FirstOrDefault(tr => tr.Id == trans.Id && tr.TransportModels.Id == nameTransportModelsFind);
                    if (tmpTransp != null)
                    {
                        transports.Add(tmpTransp);
                    }
                }
            }
            return transports == null ? 0 : transports.Count();
        }

        [HttpGet]
        public List<Promotions> GetPromotions(DateTime dateTime)
        {
            int dayOfWeek = (int)dateTime.DayOfWeek;
            if (dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }
            List<Promotions> promotions = db.Promotions.Include(pr => pr.PromotionsTransportModels.Select(tm => tm.TransportModels)).Where(day => day.DayOfWeek.Contains(dayOfWeek.ToString())).ToList();

            return promotions;
        }
    }
}
