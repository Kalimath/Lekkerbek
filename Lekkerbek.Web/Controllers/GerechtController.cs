using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Controllers
{
    public class GerechtController : Controller
    {
        // GET: GerechtController
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        // POST: GerechtController/Edit/5
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
