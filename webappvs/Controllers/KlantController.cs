using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webappvs.Models;

namespace webappvs.Controllers
{
    public class KlantController : Controller
    {
        public IActionResult Index()
        {
            List<Klant> vKLanten = new List<Klant>();

            vKLanten.Add(new Klant() { Id = 1, Naam = "De Neve", Voornaam="Jos",AangemaaktDatum = new System.DateTime(2019,1,20) });
            vKLanten.Add(new Klant() { Id = 2, Naam = "Bruynseels", Voornaam = "Jef", AangemaaktDatum = new System.DateTime(2019, 2, 4) });
            vKLanten.Add(new Klant() { Id = 3, Naam = "Naert", Voornaam = "Jan", AangemaaktDatum = new System.DateTime(2019, 1, 5) });

            return View(vKLanten);
        }
    }
}