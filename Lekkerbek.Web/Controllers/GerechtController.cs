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
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [AllowAnonymous]
    public class GerechtController : Controller
    {
        private readonly IdentityContext _context;

        public GerechtController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Gerecht
        public async Task<IActionResult> Index()
        {
            
            return View(_context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie"));
        }

        // GET: Gerecht/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").AsQueryable().FirstOrDefaultAsync(m => m.Naam == id);
            if (gerecht == null)
            {
                return NotFound();
            }

            return View(gerecht);
        }

        // GET: Gerecht/Create
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public IActionResult Create()
        {
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam");
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
                Categorie categorieGerecht = await _context.Categorie.FirstAsync(cat => cat.Naam.Equals(collection["CategorieId"]));
                gerecht.Naam = collection["Naam"];
                gerecht.CategorieId = collection["CategorieId"];
                gerecht.Prijs = Double.Parse(collection["Prijs"], new CultureInfo("en-US"));
                gerecht.Categorie = categorieGerecht; 
                _context.Add(gerecht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // GET: Gerecht/Edit/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var gerecht = _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").ToList().Find(gerecht1 => gerecht1.Naam.Equals(id));
            if (gerecht == null)
            {
                return NotFound();
            }
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // POST: Gerecht/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(string id, IFormCollection collection)
        {
            Gerecht gerecht = null; 
/*            if (id != gerecht.Naam)
            {
                return NotFound();
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    gerecht = _context.Gerecht.First(gerecht => gerecht.Naam.Equals(id)); 
                    gerecht.CategorieId = collection["CategorieId"];
                    gerecht.Categorie = await _context.Categorie.FirstAsync(cat => cat.Naam.Equals(collection["CategorieId"])); 
                    gerecht.Prijs = Double.Parse(collection["Prijs"], new CultureInfo("en-US"));

                    _context.Update(gerecht);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerechtExists(gerecht.Naam))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // GET: Gerecht/Delete/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie")
                .FirstOrDefaultAsync(m => m.Naam == id);
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gerecht = _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").ToList().Find(gerecht1 => gerecht1.Naam.Equals(id));
            _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").ToList().Remove(gerecht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerechtExists(string id)
        {
            return _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").Any(e => e.Naam == id);
        }
    }
}
