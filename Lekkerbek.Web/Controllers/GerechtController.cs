using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Lekkerbek.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [AllowAnonymous]
    public class GerechtController : Controller
    {
        private readonly IGerechtService _gerechtService;
        private readonly ICategorieService _categorieService;

        public GerechtController(IGerechtService gerechtService, ICategorieService categorieService)
        {
            _gerechtService = gerechtService;
            _categorieService = categorieService;
        }

        // GET: Gerecht
        public async Task<IActionResult> Index()
        {
            
            return View(_gerechtService.GetGerechten());
        }

        // GET: Gerecht/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string gerechtNaam)
        {
            Gerecht gerecht;
            try
            {
                gerecht = _gerechtService.GetGerecht(gerechtNaam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
            return View(gerecht);
        }

        // GET: Gerecht/Create
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public IActionResult Create()
        {
            ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam");
            return View();
        }

        // POST: Gerecht/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            Gerecht gerecht = new Gerecht(); 
            if (ModelState.IsValid) {
                try
                {
                    Categorie categorieGerecht = _categorieService.GetCategorie(collection["CategorieId"]);
                    gerecht.Naam = collection["Naam"];
                    gerecht.CategorieId = collection["CategorieId"];
                    gerecht.Prijs = Double.Parse(collection["Prijs"], new CultureInfo("en-US"));
                    gerecht.Categorie = categorieGerecht;
                    await _gerechtService.AddGerecht(gerecht);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam", gerecht.CategorieId);
                    return View(gerecht);
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // GET: Gerecht/Edit/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(string gerechtNaam)
        {

            Gerecht gerecht;
            try
            {
                gerecht = _gerechtService.GetGerecht(gerechtNaam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
            ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // POST: Gerecht/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(string gerechtNaam, IFormCollection collection)
        {
            Gerecht gerecht = null;
            if (ModelState.IsValid)
            {
                try
                {
                    gerecht = _gerechtService.GetGerecht(gerechtNaam);
                    gerecht.CategorieId = collection["CategorieId"];
                    gerecht.Categorie = _categorieService.GetCategorie(collection["CategorieId"]);
                    gerecht.Prijs = Double.Parse(collection["Prijs"], new CultureInfo("en-US"));
                    await _gerechtService.UpdateGerecht(gerecht);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam", gerecht.CategorieId);
                    return View(gerecht);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_categorieService.GetCategorieen(), "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // GET: Gerecht/Delete/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Delete(string gerechtNaam)
        {
            if (gerechtNaam == null)
            {
                return NotFound();
            }
            var gerecht = _gerechtService.GetGerecht(gerechtNaam);
            if (gerecht == null)
            {
                return NotFound();
            }

            return View(gerecht);
        }

        // POST: Gerecht/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> DeleteConfirmed(string gerechtNaam)
        {
            try
            {
                await _gerechtService.DeleteGerecht(gerechtNaam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
