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
using WebApiWithEFCache;

namespace WebApiWithEFCache.Controllers
{
    public class AirlinesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Airlines
        public IQueryable<Airline> GetAirlines()
        {
            Console.WriteLine(WebApiApplication.Cache.Count);
            return db.Airlines;
        }

        // GET: api/Airlines/5
        [ResponseType(typeof(Airline))]
        public IHttpActionResult GetAirline(string id)
        {
            Airline airline = db.Airlines.Find(id);
            if (airline == null)
            {
                return NotFound();
            }

            return Ok(airline);
        }

        // PUT: api/Airlines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAirline(string id, Airline airline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != airline.Code)
            {
                return BadRequest();
            }

            db.Entry(airline).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirlineExists(id))
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

        // POST: api/Airlines
        [ResponseType(typeof(Airline))]
        public IHttpActionResult PostAirline(Airline airline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Airlines.Add(airline);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AirlineExists(airline.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = airline.Code }, airline);
        }

        // DELETE: api/Airlines/5
        [ResponseType(typeof(Airline))]
        public IHttpActionResult DeleteAirline(string id)
        {
            Airline airline = db.Airlines.Find(id);
            if (airline == null)
            {
                return NotFound();
            }

            db.Airlines.Remove(airline);
            db.SaveChanges();

            return Ok(airline);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AirlineExists(string id)
        {
            return db.Airlines.Count(e => e.Code == id) > 0;
        }
    }
}