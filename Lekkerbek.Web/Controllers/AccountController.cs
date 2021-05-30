using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Dtos;
using Lekkerbek.Web.Models.Identity;
using Lekkerbek.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SQLitePCL;

namespace Lekkerbek.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IdentityContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;

        public AccountController(IdentityContext context, RoleManager<Role> roleManager, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Admin.ToString())|| User.IsInRole(RollenEnum.Kassamedewerker.ToString()))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Details", new { id = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(HttpContext.User)) });
            }
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Gebruiker gebruiker = null;
            if (id == 0)
            {
                return NotFound();
            }
            if (!User.IsInRole(RollenEnum.Admin.ToString()) || !User.IsInRole(RollenEnum.Kassamedewerker.ToString()))
            {
                gebruiker = await _userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                gebruiker = await _context.Gebruikers
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            if (gebruiker == null)
            {
                return NotFound();
            }

            ViewBag.Rol = _context.GebruikerHoogsteRol(id);
            //ViewBag.Rol = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(id + ""));
            return View(gebruiker);
        }

        // GET: Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return RedirectToPage("Register");
        }

        // GET: Account/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return RedirectToPage("Login");
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            return RedirectToRoute("Logout");
        }

        [Authorize(Roles = "Admin,Kassamedewerker")]
        public IActionResult Create()
        {
            ViewBag.Rollen = new SelectList(Enum.GetValues<RollenEnum>());
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Create([Bind("UserName,Email,Adres,Geboortedatum,Getrouwheidsscore,IsProfessional,BtwNummer,FirmaNaam,Rol,PasswordHash")] GebruikerMetRolDto gebruikerDto)
        {
            if (ModelState.IsValid)
            {
                var nieuweGebruiker = new Gebruiker()
                {
                    UserName = gebruikerDto.UserName,
                    Email = gebruikerDto.Email,
                    Adres = gebruikerDto.Adres,
                    Geboortedatum = gebruikerDto.Geboortedatum,
                    Getrouwheidsscore = gebruikerDto.Getrouwheidsscore,
                    IsProfessional = gebruikerDto.IsProfessional,
                    BtwNummer = gebruikerDto.BtwNummer,
                    FirmaNaam = gebruikerDto.FirmaNaam,
                    PasswordHash = gebruikerDto.PasswordHash
                };
                var user = await _userManager.CreateAsync(nieuweGebruiker, gebruikerDto.PasswordHash);
                await _userManager.AddToRoleAsync(nieuweGebruiker, gebruikerDto.Rol);
                _context.Add(nieuweGebruiker);
                return RedirectToAction(nameof(Index));
            }
            return View(gebruikerDto);
        }

        // GET: Account/Edit/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruikers.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }
            ViewBag.Rol = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(id + ""));
            return View(gebruiker);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(int id, [Bind("UserName,Email,Adres,Geboortedatum,Getrouwheidsscore,IsProfessional,BtwNummer,FirmaNaam")] Gebruiker gebruiker)
        {
            if (id != gebruiker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gebruiker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GebruikerExists(gebruiker.Id))
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
            return View(gebruiker);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruikers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            return View(gebruiker);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);
            _context.Gebruikers.Remove(gebruiker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.Id == id);
        }

        public List<GebruikerMetRolViewModel> GebruikersMetRolViewModels()
        {
            IQueryable<GebruikerMetRolViewModel> viewmodels = from u in _context.Gebruikers
                select new GebruikerMetRolViewModel()
                {
                    Id = u.Id,
                    Gebruikersnaam = u.UserName,
                    Email = u.Email,
                    Adres = u.Adres,
                    Geboortedatum = u.Geboortedatum,
                    Rol = _context.GebruikerHoogsteRol(u.Id)
                };

            return viewmodels.ToList();
        }

        public JsonResult LaadAlleGebruikersMetRol()
        {
            return Json(new { data = GebruikersMetRolViewModels() });
        }
    }
}
