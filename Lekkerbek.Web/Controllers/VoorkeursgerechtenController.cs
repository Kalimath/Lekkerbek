using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
    public class VoorkeursgerechtenController : Controller
    {
        private readonly IdentityContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;

        public VoorkeursgerechtenController(IdentityContext context, RoleManager<Role> rolemanager, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _roleManager = rolemanager;
            _userManager = userManager;
        }
        // GET: VoorkeursgerechtenController
        public async Task<ActionResult> Index()
        {
           
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                ViewBag.User = currentUser.Id;

                return View(_context.VoorkeursGerechtenVanKlanten(currentUser.Id));
            }
            else
            {

                return View(_context.Gebruikers.Include("Voorkeursgerechten").ToList()); 
            }
        }
        // GET: VoorkeursgerechtenController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VoorkeursgerechtenController/Create
        public async Task<ActionResult> Create()
        {
            var beschikbareGerechten = _context.Gerechten.ToList();
            ViewData["Naam"] = new SelectList(beschikbareGerechten, "Naam", "Naam"); 

            return View();
        }

        // POST: VoorkeursgerechtenController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Gerecht gerecht = new Gerecht(); 
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var gerechtvandb = await _context.Gerechten.FirstAsync(g => g.Naam.Equals(collection["Naam"])); 
                gerecht = gerechtvandb;
                _context.Gebruikers.First(klant => klant.Id == currentUser.Id).Voorkeursgerechten.Add(gerecht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        // GET: VoorkeursgerechtenController/Delete/5
        public async Task<ActionResult> Delete(String id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var gerecht = _context.Gerechten.First(g => g.Naam.Equals(id));            
            return View(gerecht);
        }

        // POST: VoorkeursgerechtenController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(String id, IFormCollection collection)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var gerecht = _context.Gerecht.First(g => g.Naam.Equals(id)); 
                _context.VoorkeursGerechtenVanKlanten(currentUser.Id).Remove(gerecht);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
