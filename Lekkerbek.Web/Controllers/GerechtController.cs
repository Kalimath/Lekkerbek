using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Controllers
{
    public class GerechtController : Controller
    {
        private readonly BestellingDbContext _context;

        public GerechtController(BestellingDbContext context)
        {
            _context = context;
        }

        // GET: Gerechts
        public async Task<IActionResult> Index()
        {
            var bestellingDbContext = _context.Gerecht.Include(g => g.Categorie);
            return View(await bestellingDbContext.ToListAsync());
        }

        // GET: Gerechts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerecht
                .Include(g => g.Categorie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gerecht == null)
            {
                return NotFound();
            }

            return View(gerecht);
        }

        // GET: Gerechts/Create
        public IActionResult Create()
        {
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam");
            return View();
        }

        // POST: Gerechts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Omschrijving,CategorieId,Prijs")] Gerecht gerecht)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gerecht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // GET: Gerechts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerecht.FindAsync(id);
            if (gerecht == null)
            {
                return NotFound();
            }
            ViewData["CategorieId"] = new SelectList(_context.Categorie, "Naam", "Naam", gerecht.CategorieId);
            return View(gerecht);
        }

        // POST: Gerechts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Omschrijving,CategorieId,Prijs")] Gerecht gerecht)
        {
            if (id != gerecht.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerecht);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerechtExists(gerecht.Id))
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

        // GET: Gerechts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerecht = await _context.Gerecht
                .Include(g => g.Categorie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gerecht == null)
            {
                return NotFound();
            }

            return View(gerecht);
        }

        // POST: Gerechts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gerecht = await _context.Gerecht.FindAsync(id);
            _context.Gerecht.Remove(gerecht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerechtExists(int id)
        {
            return _context.Gerecht.Any(e => e.Id == id);
        }
    }
}
