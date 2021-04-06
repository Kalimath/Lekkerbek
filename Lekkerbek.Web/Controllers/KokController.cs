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
    public class KokController : Controller
    {
        private readonly IdentityContext _context;

        public KokController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Kok
        public async Task<IActionResult> Index()
        {
            return View(await _context.Koks.ToListAsync());
        }

        // GET: Kok/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kok = await _context.Koks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kok == null)
            {
                return NotFound();
            }

            return View(kok);
        }

        // GET: Kok/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kok/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam")] Kok kok)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kok);
        }

        // GET: Kok/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kok = await _context.Koks.FindAsync(id);
            if (kok == null)
            {
                return NotFound();
            }
            return View(kok);
        }

        // POST: Kok/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam")] Kok kok)
        {
            if (id != kok.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KokExists(kok.Id))
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
            return View(kok);
        }

        // GET: Kok/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kok = await _context.Koks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kok == null)
            {
                return NotFound();
            }

            return View(kok);
        }

        // POST: Kok/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kok = await _context.Koks.FindAsync(id);
            _context.Koks.Remove(kok);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KokExists(int id)
        {
            return _context.Koks.Any(e => e.Id == id);
        }
    }
}
