using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Controllers
{
    public class KlantController : Controller
    {
        // GET: KlantController
        public ActionResult Index()
        {
            return View(KlantenDBTemp.getKlanten());
        }

        // GET: KlantController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KlantController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KlantController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Klant newKlant = new Klant()
                {
                    Naam = collection["Naam"],
                    Adres = collection["Adres"],
                    Geboortedatum = DateTime.Parse(collection["Geboortedatum"]),
                    Getrouwheidsscore = Int32.Parse(collection["Getrouwheidsscore"])
                };
                KlantenDBTemp.AddKlant(newKlant);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: KlantController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(KlantenDBTemp.GetKlant(id));
        }

        // POST: KlantController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                KlantenDBTemp.UpdateKlant(id, collection["Naam"], collection["Adres"],
                                    DateTime.Parse(collection["Geboortedatum"]), 
                                    Int32.Parse(collection["Getrouwheidsscore"]));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: KlantController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KlantController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
