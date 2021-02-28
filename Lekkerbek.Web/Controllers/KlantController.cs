using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Controllers
{
    public class KlantController : Controller
    {
        // GET: KlantController
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        // POST: KlantController/Edit/5
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
