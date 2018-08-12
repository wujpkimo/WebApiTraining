using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProductsApp.Models
{
    [MyException]
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// 取得Client資料
        /// </summary>
        /// <returns></returns>
        [Route("")]
        // GET: api/Clients
        public IHttpActionResult GetClient()
        {
            return Ok(db.Client.Take(10));
        }

        // GET: api/Clients/5
        [Route("{id:int}", Name = nameof(GetNameById))]
        [ResponseType(typeof(Client))]
        public HttpResponseMessage GetNameById(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, client);
        }

        [Route("{id:int}/orders", Name = nameof(GetClientOrders))]
        [ResponseType(typeof(Client))]
        public HttpResponseMessage GetClientOrders(int id)
        {
            var orders = db.Order.Where(p => p.ClientId == id);
            return new HttpResponseMessage()
            {
                ReasonPhrase = "HELLO",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IQueryable<Order>>(orders,
                    GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }

        [Route("{id:int}/orders/{date:datetime}", Name = nameof(GetClientOrdersByDate1))]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrdersByDate1(int id, DateTime date)
        {
            var next_day = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate > date && p.OrderDate <= next_day);
            return Ok(orders.ToList());
        }

        [Route("{id:int}/orders/{*date:datetime}", Name = nameof(GetClientOrdersByDate2))]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrdersByDate2(int id, DateTime date)
        {
            var next_day = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate > date && p.OrderDate <= next_day);
            return Ok(orders.ToList());
        }

        [Route("{id}")]
        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        // POST: api/Clients
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetNameById), new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [Route("{id}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}