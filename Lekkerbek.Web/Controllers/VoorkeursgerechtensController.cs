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
    public class VoorkeursgerechtensController : Controller
    {
        private readonly IdentityContext _context;

        public VoorkeursgerechtensController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Voorkeursgerechtens
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.Voorkeursgerechten.Include(v => v.Klant);
            return View(await identityContext.ToListAsync());
        }

        // GET: Voorkeursgerechtens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voorkeursgerechten = await _context.Voorkeursgerechten
                .Include(v => v.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voorkeursgerechten == null)
            {
                return NotFound();
            }

            return View(voorkeursgerechten);
        }

        // GET: Voorkeursgerechtens/Create
        public async Task<IActionResult> Create(int Id)
        {
            var klant = await _context.Klanten.FirstOrDefaultAsync(k => k.Id == Id);
            ViewData["KlanId"] = Id; 
            ViewData["Klant"] = klant.Naam;                 
            ViewData["GerechtId"] = new SelectList(_context.Gerechten, "Naam", "Naam");
            return View();
        }

        // POST: Voorkeursgerechtens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create(int id, IFormCollection collection)
        {
           int nextId = _context.Voorkeursgerechten.Max(m => m.Id) + 1;
           Voorkeursgerechten voorkeursgerechten = new Voorkeursgerechten { Id = nextId, GerechtId = collection["GerechtId"], KlantId = id };

            if (ModelState.IsValid)
            {
                _context.Add(voorkeursgerechten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voorkeursgerechten);
        }*/

        public async Task<IActionResult> Create([Bind("Id,GerechtId,KlantId")] Voorkeursgerechten voorkeursgerechten)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(voorkeursgerechten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voorkeursgerechten);
        }


        // GET: Voorkeursgerechtens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voorkeursgerechten = await _context.Voorkeursgerechten.FindAsync(id);
            if (voorkeursgerechten == null)
            {
                return NotFound();
            }
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Discriminator", voorkeursgerechten.KlantId);
            return View(voorkeursgerechten);
        }

        // POST: Voorkeursgerechtens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GerechtId,KlantId")] Voorkeursgerechten voorkeursgerechten)
        {
            if (id != voorkeursgerechten.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voorkeursgerechten);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoorkeursgerechtenExists(voorkeursgerechten.Id))
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
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Discriminator", voorkeursgerechten.KlantId);
            return View(voorkeursgerechten);
        }

        // GET: Voorkeursgerechtens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voorkeursgerechten = await _context.Voorkeursgerechten
                .Include(v => v.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voorkeursgerechten == null)
            {
                return NotFound();
            }

            return View(voorkeursgerechten);
        }

        // POST: Voorkeursgerechtens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voorkeursgerechten = await _context.Voorkeursgerechten.FindAsync(id);
            _context.Voorkeursgerechten.Remove(voorkeursgerechten);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoorkeursgerechtenExists(int id)
        {
            return _context.Voorkeursgerechten.Any(e => e.Id == id);
        }
    }
}
