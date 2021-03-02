using Lekkerbek.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Controllers
{
    public class BestellingController : Controller
    {
        // GET: BestellingController
        public ActionResult Index()
        {
            return View(BestellingenDbTemp.GetBestellingen());
        }

        // GET: BestellingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BestellingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BestellingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: BestellingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BestellingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: BestellingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BestellingController/Delete/5
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
