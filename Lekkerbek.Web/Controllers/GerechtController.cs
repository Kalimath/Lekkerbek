using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Controllers
{
    public class GerechtController : Controller
    {
        public GerechtController()
        {
            
        }
        // GET: GerechtController
        public ActionResult Index()
        {
            return View(GerechtenDBTemp.getGerechten());
        }

        // GET: GerechtController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GerechtController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GerechtController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Gerecht newGerecht = new Gerecht()
                {
                    Omschrijving = collection["Omschrijving"],
                    Categorie = Enum.Parse<CategorieEnum>(((string)collection["Categorie"]).ToLower()),
                    Prijs = Double.Parse(collection["Prijs"], new CultureInfo("en-US"))
                };
                GerechtenDBTemp.AddGerecht(newGerecht);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GerechtController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GerechtenDBTemp.GetGerecht(id));
        }

        // POST: GerechtController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                GerechtenDBTemp.UpdateGerecht(id, collection["Omschrijving"], 
                                            Enum.Parse<CategorieEnum>(((string)collection["Categorie"]).ToLower()),
                                            Double.Parse(collection["Prijs"], new CultureInfo("en-US")));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GerechtController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GerechtController/Delete/5
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
