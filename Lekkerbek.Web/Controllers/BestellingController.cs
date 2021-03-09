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

namespace Lekkerbek.Web.Controllers
{
    public class BestellingController : Controller
    {
        private readonly BestellingDbContext _context;

        public BestellingController(BestellingDbContext context)
        {
            _context = context;
        }

        // GET: Bestelling
        public async Task<IActionResult> Index()
        {
            var bestellingDbContext = _context.Bestellingen.Include(b => b.Klant);
            return View(await bestellingDbContext.ToListAsync());
        }

        // GET: Bestelling/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen
                .Include(b => b.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }

        // GET: Bestelling/Create
        public IActionResult Create()
        {
            ViewData["KlantNaam"] = new SelectList(_context.Klanten, "Naam", "Naam");
            ViewData["AlleGerechten"] = new SelectList(_context.Gerechten, "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(Tijdstippen.tijdslotten, "Tijdstip", "Tijdstip"); 
          
            return View();
        }

        // POST: Bestelling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*[Bind("Id,Leverdatum,Opmerkingen,AantalMaaltijden,KlantNaam")] Bestelling bestelling*/
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            Bestelling bestelling = new Bestelling();
            if (ModelState.IsValid)
            {
                bestelling = new Bestelling()
                {
                    AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]),
                    GerechtenLijst = new List<Gerecht>(),
                    Id = _context.Bestellingen.ToList().Count + 1,
                    Klant = (Klant)_context.Klanten.Where(klant => klant.Naam.Equals(collection["KlantNaam"])),
                    Leverdatum = DateTime.Parse(collection["Leverdatum"]).Date,
                    KlantNaam = collection["KlantNaam"],
                };
                bestelling.GerechtenLijst.Add((Gerecht)_context.Gerechten.Where((gerecht => gerecht.Naam.Equals(collection["GerechtenLijst"]))));
                _context.Add(bestelling);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlantNamen"] = new SelectList(_context.Klanten, "Naam", "Naam");
            
            ViewData["Tijdslot"] = new SelectList(Tijdstippen.tijdslotten, "Tijdstip", "Tijdstip", bestelling.Tijdslot);
            return RedirectToAction("Index");
        }

        // GET: Bestelling/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen.FindAsync(id);
            if (bestelling == null)
            {
                return NotFound();
            }
            ViewData["KlantNaam"] = new SelectList(_context.Klanten, "Id", "Id", bestelling.KlantNaam);
            ViewData["Tijdslot"] = new SelectList(Tijdstippen.tijdslotten, "Tijdstip", "Tijdstip", bestelling.Tijdslot);
            return View(bestelling);
        }

        // POST: Bestelling/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Leverdatum,Opmerkingen,AantalMaaltijden,KlantNaam, Tijdslot")] Bestelling bestelling)
        {
            if (id != bestelling.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellingExists(bestelling.Id))
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
            ViewData["KlantNaam"] = new SelectList(_context.Klanten, "Id", "Id", bestelling.KlantNaam);
            ViewData["Tijdslot"] = new SelectList(Tijdstippen.tijdslotten, "Tijdstip", "Tijdstip", bestelling.Tijdslot);
            return View(bestelling);
        }

        // GET: Bestelling/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen
                .Include(b => b.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }

        // POST: Bestelling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);
            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }
    }
}
