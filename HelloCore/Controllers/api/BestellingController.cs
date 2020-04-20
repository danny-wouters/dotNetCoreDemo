using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloCore.Data;
using HelloCore.Data.UnitOfWork;
using HelloCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloCore.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestellingController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BestellingController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Bestelling> Get()
        {
            var vBestellingen = _uow.BestellingRepository.GetAll();
            return vBestellingen;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Bestelling Get(int id)
        {
            return _uow.BestellingRepository.GetById(id);
        }

        // GET: api/<controller>/bestellingen
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("bestellingen")]
        public IEnumerable<Bestelling> GetBestellingen()
        {
            return _uow.BestellingRepository.GetAll();
        }

        //GET: api/<controller>
        [HttpGet("search")]
        public IEnumerable<Bestelling> GetSearch()
        {
            //Queryable doesn't get executed until we access result (.ToList())
            var vBestellingen = _uow.BestellingRepository.GetAll();
            vBestellingen = vBestellingen.Where(b => b.Prijs >= 15);
            vBestellingen = vBestellingen.Where(b => b.Artikel.Length > 2);

            return vBestellingen.ToList();
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<Bestelling> PostBestelling([FromBody]Bestelling value)
        {
            try
            {
                _uow.BestellingRepository.Create(value);
                _uow.Save();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return CreatedAtAction(nameof(Get), new { id = value.BestellingID }, value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult PutBestelling(int id, [FromBody]Bestelling value)
        {
            if (id != value.BestellingID)
            {
                return BadRequest();
            }

            var vOldBestelling = _uow.BestellingRepository.GetById(id);

            vOldBestelling.Artikel = value.Artikel;
            vOldBestelling.KlantID = value.KlantID;
            vOldBestelling.Prijs = value.Prijs;

            _uow.BestellingRepository.Update(id, vOldBestelling);
            _uow.Save();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteBestelling(int id)
        {
            var vBestelling = _uow.BestellingRepository.GetById(id);

            if (vBestelling == null)
            {
                return NotFound();
            }

            _uow.BestellingRepository.Delete(id);
            _uow.Save();

            return NoContent();
        }
    }
}
