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
    public class AircraftController : ApiController
    {
        private Entities db1 = new Entities();

        // GET: api/Aircraft
        public IQueryable<Aircraft> GetAircraft()
        {
            return db1.Aircraft;
        }

        // GET: api/Aircraft/5
        [ResponseType(typeof(Aircraft))]
        public IHttpActionResult GetAircraft(int id)
        {
            Aircraft aircraft = db1.Aircraft.Find(id);
            if (aircraft == null)
            {
                return NotFound();
            }

            return Ok(aircraft);
        }

        // PUT: api/Aircraft/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAircraft(int id, Aircraft aircraft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aircraft.Id)
            {
                return BadRequest();
            }

            db1.Entry(aircraft).State = EntityState.Modified;

            try
            {
                db1.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AircraftExists(id))
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

        // POST: api/Aircraft
        [ResponseType(typeof(Aircraft))]
        public IHttpActionResult PostAircraft(Aircraft aircraft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db1.Aircraft.Add(aircraft);
            db1.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aircraft.Id }, aircraft);
        }

        // DELETE: api/Aircraft/5
        [ResponseType(typeof(Aircraft))]
        public IHttpActionResult DeleteAircraft(int id)
        {
            Aircraft aircraft = db1.Aircraft.Find(id);
            if (aircraft == null)
            {
                return NotFound();
            }

            db1.Aircraft.Remove(aircraft);
            db1.SaveChanges();

            return Ok(aircraft);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db1.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AircraftExists(int id)
        {
            return db1.Aircraft.Count(e => e.Id == id) > 0;
        }
    }
}