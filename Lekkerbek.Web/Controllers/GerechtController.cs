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

namespace Lekkerbek.Web.Controllers
{
    public class GerechtController : Controller
    {
        private readonly BestellingDbContext _context;

        public GerechtController(BestellingDbContext context)
        {
            _context = context;
        }

        // GET: Gerecht
        public async Task<IActionResult> Index()
        {
            var bestellingDbContext = _context.Gerechten.Include(g => g.Categorie);
            return View(await bestellingDbContext.ToListAsync());
        }

        // GET: Gerecht/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerechten
                .Include(g => g.Categorie)
                .FirstOrDefaultAsync(m => m.Naam == id);
            if (gerecht == null)
            {
                return NotFound();
            }

            return View(gerecht);
        }

        // GET: Gerecht/Create
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
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerechten.FindAsync(id);
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerechten
                .Include(g => g.Categorie)
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gerecht = await _context.Gerechten.FindAsync(id);
            _context.Gerechten.Remove(gerecht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerechtExists(string id)
        {
            return _context.Gerechten.Any(e => e.Naam == id);
        }
    }
}
