﻿using System;
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
using Lekkerbek.Web.Services;
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
        private readonly IGebruikerService _gebruikerService;
        private readonly UserManager<Gebruiker> _userManager;

        public AccountController(IGebruikerService gebruikerService, UserManager<Gebruiker> userManager)
        {
            _gebruikerService = gebruikerService;
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
                return RedirectToAction("Details", new { id = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User)).Id });
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

            var hoogsteRolVanGebruiker = _gebruikerService.GetHoogsteRolVanGebruiker(_gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User)).Id);

            if (!hoogsteRolVanGebruiker.Equals(RollenEnum.Admin.ToString()) && !hoogsteRolVanGebruiker.Equals(RollenEnum.Kassamedewerker.ToString()))
            {
                gebruiker = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
            }
            else
            {
                gebruiker = _gebruikerService.GetGebruiker(id);
            }
            if (gebruiker == null)
            {
                return NotFound();
            }

            ViewBag.Rol = _gebruikerService.GetHoogsteRolVanGebruiker(id);
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
                await _gebruikerService.AddGebruiker(nieuweGebruiker,  nieuweGebruiker.PasswordHash, gebruikerDto.Rol);
                return RedirectToAction(nameof(Index));
            }
            return View(gebruikerDto);
        }

        // GET: Account/Edit/5
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> Edit(int? id)
        {
            Gebruiker gebruiker;
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                gebruiker = _gebruikerService.GetGebruiker((int) id);
            }

            if (gebruiker == null)
            {
                return NotFound();
            }
            ViewBag.Rol = _gebruikerService.GetGebruikerRollen();
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
                    await _gebruikerService.UpdateGebruiker(gebruiker);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_gebruikerService.GebruikerExists(id))
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

            var gebruiker = _gebruikerService.GetGebruiker((int) id);
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
            try
            {
               await _gebruikerService.DeleteGebruiker(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public List<GebruikerMetRolViewModel> GebruikersMetRolViewModels()
        {
            IQueryable<GebruikerMetRolViewModel> viewmodels = from u in _gebruikerService.GetGebruikers().AsQueryable()
                select new GebruikerMetRolViewModel()
                {
                    Id = u.Id,
                    Gebruikersnaam = u.UserName,
                    Email = u.Email,
                    Adres = u.Adres,
                    Geboortedatum = u.Geboortedatum,
                    Rol = _gebruikerService.GetHoogsteRolVanGebruiker(u.Id)
                };

            return viewmodels.ToList();
        }

        public JsonResult LaadAlleGebruikersMetRol()
        {
            return Json(new { data = GebruikersMetRolViewModels() });
        }
    }
}
