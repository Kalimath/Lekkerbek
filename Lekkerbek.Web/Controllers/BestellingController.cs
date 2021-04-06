using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Lekkerbek.Web.Controllers
{
    public class BestellingController : Controller
    {
        private readonly IdentityContext _context;

        public BestellingController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Bestelling
        public async Task<IActionResult> Index()
        {
            var bestellingDbContext = _context.Bestellingen.Include(b => b.Klant);
            /*bestellingDbContext.ForEachAsync(bestelling => bestelling.GerechtenLijst = _context.Gerechten.Where(gerecht => gerecht.Bestellingen.Any(bestellingTemp => bestellingTemp.Id == bestelling.Id)).ToList())*/;
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
            bestelling.GerechtenLijst = _context.Gerechten.Where(gerecht => gerecht.Bestellingen.Any(bestellingTemp => bestellingTemp.Id == bestelling.Id)).ToList();
            bestelling.Klant.Bestellingen = _context.GebruikersMetRolKlant().First(klant => klant.Id == bestelling.KlantId).Bestellingen;
            if (bestelling == null)
            {
                return NotFound();
            }
            return View(bestelling);
        }

        // GET: Bestelling/Create
        public IActionResult Create()
        {
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten, "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten().Where(tijdslot => tijdslot.IsVrij), "Tijdstip", "Tijdstip");
            return View();
        }

        // POST: Bestelling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            Bestelling bestelling = new Bestelling();
            if (ModelState.IsValid)
            {
                Gebruiker klantVanBestelling = await _context.GebruikersMetRolKlant().AsQueryable().FirstAsync(klant => klant.UserName.Trim().ToLower().Equals(collection["Gebruiker.Naam"].ToString().Trim().ToLower()));
                bestelling = new Bestelling()
                {
                    AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]),
                    GerechtenLijst = new List<Gerecht>(),
                    Klant = klantVanBestelling,
                    Opmerkingen = collection["Opmerkingen"],
                    Levertijd = DateTime.Parse(collection["Levertijd"]),
                    KlantId = klantVanBestelling.Id,
                    Tijdslot = new Tijdslot(DateTime.Parse(collection["Tijdslot"])),
                    /*_context.Tijdsloten.First(tijdslot => tijdslot.Tijdstip == DateTime.Parse(collection["Tijdslot"]) && tijdslot.IsVrij)*/
        };
                _context.AlleVrijeTijdsloten().ToList().First(tijdslot => tijdslot.Tijdstip == DateTime.Parse(collection["Tijdslot"])).IsVrij = false;
                IEnumerable<string> gerechtNamen = (ICollection<string>)collection["GerechtenLijst"];
                bestelling.GerechtenLijst = await _context.Gerechten.Where(gerecht=> gerechtNamen.Contains(gerecht.Naam)).ToListAsync();
                _context.Add(bestelling);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlantNamen"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten(), "Tijdstip", "Tijdstip", bestelling.Tijdslot.Tijdstip);
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
            if (DateTime.Now > bestelling.Levertijd.AddHours(-1))
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Alletijdsloten().Find(tijdslot => tijdslot.Tijdstip == bestelling.Tijdslot.Tijdstip).IsVrij = true;
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten, "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten(), "Tijdstip", "Tijdstip");
            return View(bestelling);
        }

        // POST: Bestelling/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            Bestelling bestelling = null;

            if (ModelState.IsValid)
            {
                try
                {
                    Gebruiker klantVanBestelling = await _context.GebruikersMetRolKlant().AsQueryable().FirstAsync(klant => klant.UserName.Trim().ToLower().Equals(collection["Klant.Naam"].ToString().Trim().ToLower()));
                    bestelling = _context.Bestellingen.First(bestelling => bestelling.Id == id);
                    bestelling.AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]);
                    bestelling.Klant = klantVanBestelling;
                    bestelling.Opmerkingen = collection["Opmerkingen"];
                    bestelling.Levertijd = DateTime.Parse(collection["Levertijd"]);
                    bestelling.KlantId = klantVanBestelling.Id;
                    bestelling.Tijdslot = new Tijdslot(DateTime.Parse(collection["Tijdslot"]));

                    //Maak tijdslot weer beschikbaar als dit is veranderd of bestelling is verwijderd.
                    if (collection["Tijdslot"] != bestelling.Tijdslot)
                    {
                        _context.Alletijdsloten().ToList().Find(tijdslot => tijdslot.Tijdstip == _context.Bestellingen.Find(id).Tijdslot.Tijdstip).IsVrij = true;
                    }
                    _context.Alletijdsloten().First(tijdslot => tijdslot.Tijdstip == DateTime.Parse(collection["Tijdslot"]) && tijdslot.IsVrij).IsVrij = false;
                    IEnumerable<string> gerechtNamen = (ICollection<string>)collection["GerechtenLijst"];
                    var nieuweGerechten = _context.Gerechten.Where(gerecht => gerechtNamen.Contains(gerecht.Naam)).ToList()
                        .AsQueryable();
                    
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten, "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten(), "Tijdstip", "Tijdstip");
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

            //Maak tijdslot weer beschikbaar als dit is veranderd of bestelling is verwijderd.
            if (bestelling.Tijdslot != _context.Bestellingen.Find(id).Tijdslot)
            {
                _context.Alletijdsloten().ToList().Find(tijdslot => tijdslot.Tijdstip == _context.Bestellingen.Find(id).Tijdslot.Tijdstip).IsVrij = true;
            }

            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MijnBestellingen(int? klantId)
        {
            var klanten = _context.GebruikersMetRolKlant().AsQueryable();
            return View(klanten.First((klant => klant.Id == klantId)).Bestellingen);
        }
    }
}
