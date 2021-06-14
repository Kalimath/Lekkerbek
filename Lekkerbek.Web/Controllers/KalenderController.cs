using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Services;
using Lekkerbek.Web.ViewModels.Kalender;

namespace Lekkerbek.Web.Controllers
{
    public class KalenderController : Controller
    {
        private readonly IKalenderService _kalenderService;
        public KalenderController (IKalenderService kalenderService)
        {
            _kalenderService = kalenderService;
        }

        // GET: KalenderController
        public ActionResult Index()
        {
            var model = new KalenderIndexViewModel();
            return View(model);
        }

        // GET: KalenderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KalenderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KalenderController/Create
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

        // GET: KalenderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KalenderController/Edit/5
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

        // GET: KalenderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KalenderController/Delete/5
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
