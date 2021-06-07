using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.ViewModels.OpeningsUur;
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [Authorize]
    public class OpeningsUursController : Controller
    {
        private readonly IdentityContext _context;

        public OpeningsUursController(IdentityContext context)
        {
            _context = context;
        }

        // GET: OpeningsUurs
        [AllowAnonymous]
        public  IActionResult Index()
        {
            var model = from c in _context.OpeningsUren
                        select new OpeningsUurViewModel()
                        {
                            Id = c.Id,
                            Dag = c.Dag,
                            Uur = c.Uur,
                            Datum = c.Datum
                        };
            return View(model);
        }

        // GET: OpeningsUurs/Details/5
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingsUur = await _context.OpeningsUren
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openingsUur == null)
            {
                return NotFound();
            }

            return View(openingsUur);
        }

        // GET: OpeningsUurs/Create
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OpeningsUurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public async Task<IActionResult> Create([Bind("Id,Dag,Uur,Datum")] OpeningsUur openingsUur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(openingsUur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(openingsUur);
        }

        // GET: OpeningsUurs/Edit/5
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingsUur = await _context.OpeningsUren.FindAsync(id);
            if (openingsUur == null)
            {
                return NotFound();
            }
            return View(openingsUur);
        }

        // POST: OpeningsUurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dag,Uur,Datum")] OpeningsUur openingsUur)
        {
            if (id != openingsUur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(openingsUur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpeningsUurExists(openingsUur.Id))
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
            return View(openingsUur);
        }

        // GET: OpeningsUurs/Delete/5
        [Authorize(Roles = "Adim,Kassamedewerker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingsUur = await _context.OpeningsUren
                .FirstOrDefaultAsync(m => m.Id == id);
            if (openingsUur == null)
            {
                return NotFound();
            }

            return View(openingsUur);
        }

        // POST: OpeningsUurs/Delete/5
        [Authorize(Roles = "Adim,Kassamedewerker")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var openingsUur = await _context.OpeningsUren.FindAsync(id);
            _context.OpeningsUren.Remove(openingsUur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpeningsUurExists(int id)
        {
            return _context.OpeningsUren.Any(e => e.Id == id);
        }
    }
}
