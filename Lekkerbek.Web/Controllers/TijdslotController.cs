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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Kassamedewerker,Kok")]
    public class TijdslotController : Controller
    {
        private readonly IdentityContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;

        public TijdslotController(IdentityContext context, RoleManager<Role> roleManager, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Tijdslot
        
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Kok.ToString()))
            {
                var currentUser = (await _userManager.GetUserAsync(HttpContext.User));
                var tijdsloten = _context.TijdslotenToegankelijkVoorKok(currentUser).Result;
                return View(tijdsloten.OrderBy(tijdslot => tijdslot.Tijdstip));
            }
            else
            {
                return View(await _context.Tijdslot.Include("InGebruikDoorKok").OrderBy(tijdslot => tijdslot.Tijdstip).ToListAsync());
            }
        }

        // GET: Tijdslot/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdslot.Include("InGebruikDoorKok")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tijdslot == null)
            {
                return NotFound();
            }
            ViewBag.Bestelling = BestellingVanTijdslot(tijdslot.Id);
            return View(tijdslot);
        }

        [Authorize(Roles = "Admin,Kassamedewerker")]
        // GET: Tijdslot/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tijdslot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Create([Bind("Id,Tijdstip,IsVrij")] Tijdslot tijdslot)
        {
            if (ModelState.IsValid)
            {
                //TODO
                _context.Add(tijdslot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tijdslot);
        }

        // POST: Tijdslot/Toegewezen/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Kok")]
        public async Task<IActionResult> Toegewezen(int id)
        {
            try
            {
                var tijdslot = _context.Tijdslot.Find(id);
                if(tijdslot.InGebruikDoorKok == null) tijdslot.InGebruikDoorKok = await _userManager.GetUserAsync(HttpContext.User);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Details), new {id});
        }


        // GET: Tijdslot/Edit/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdslot.FindAsync(id);
            if (tijdslot == null)
            {
                return NotFound();
            }
            return View(tijdslot);
        }

        // POST: Tijdslot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tijdstip,IsVrij")] Tijdslot tijdslot)
        {
            if (id != tijdslot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tijdslot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TijdslotExists(tijdslot.Id))
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
            return View(tijdslot);
        }

        // GET: Tijdslot/Delete/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tijdslot = await _context.Tijdslot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tijdslot == null)
            {
                return NotFound();
            }

            return View(tijdslot);
        }

        // POST: Tijdslot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tijdslot = await _context.Tijdslot.FindAsync(id);
            _context.Tijdslot.Remove(tijdslot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TijdslotExists(int id)
        {
            return _context.Tijdslot.Any(e => e.Id == id);
        }

        public Bestelling BestellingVanTijdslot(int tijdslotId)
        {
            Bestelling bestelling = null;
            try
            {
                bestelling = _context.Bestellingen.Include("GerechtenLijst").FirstOrDefault(bestelling => bestelling.Tijdslot.Id == tijdslotId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bestelling;
        }
    }
}
