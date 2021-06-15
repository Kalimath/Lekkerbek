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
using Lekkerbek.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static Lekkerbek.Web.Models.Identity.RollenEnum;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Klant")]
    public class KlachtenController : Controller
    {
        private readonly IKlachtenService _klachtenService;
        private readonly IGebruikerService _gebruikerService;
        private readonly IMailService _mailService;
        private readonly UserManager<Gebruiker> _userManager;

        private readonly List<string> _blackList = new List<string>()
        {
            "henk.verelst@thix-it.be","henk.verelst@ucll.be","info@lekkerbek.be"
        };

        public KlachtenController(IKlachtenService klachtenService, UserManager<Gebruiker> userManager, IGebruikerService gebruikerService, IMailService mailService)
        {
            _klachtenService = klachtenService;
            _userManager = userManager;
            _gebruikerService = gebruikerService;
            _mailService = mailService;
        }

        // GET: Klachten
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
                return View(_klachtenService.GetKlachtenVanKlant(currentUser.Id));
            }
            else
            {
                return View(_klachtenService.GetKlachten());
            }
        }

        // GET: Klachten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klacht = _klachtenService.GetKlacht((int)id);
            if (klacht == null)
            {
                return NotFound();
            }

            return View(klacht);
        }
        [Authorize(Roles = "Klant")]
        // GET: Klachten/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Klachten/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klant")]
        public async Task<IActionResult> Create([Bind("Id,Onderwerp,Omschrijving")] Klacht klacht)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
                klacht.Klant = currentUser;
                await _klachtenService.AddKlacht(klacht);
                var admins = _gebruikerService.GetGebruikers().Where(gebruiker =>
                    _gebruikerService.GetHoogsteRolVanGebruiker(gebruiker.Id)
                        .Equals(RollenEnum.Admin.ToString())).ToList();
                List<Gebruiker> editedAdmins = new List<Gebruiker>();
                editedAdmins.AddRange(collection: admins);
                foreach (var admin in admins)
                {
                    if (_blackList.Contains(admin.Email)&& DateTime.Now < new DateTime(2021,06,15))
                    {
                        editedAdmins.Remove(admin);
                    }
                }

                await _mailService.SendKlacht(klacht, editedAdmins);
                return RedirectToAction(nameof(Index));
            }
            return View(klacht);
        }

        // GET: Klachten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klacht = _klachtenService.GetKlacht((int)id);
            if (klacht == null)
            {
                return NotFound();
            }

            return View(klacht);
        }

        // POST: Klachten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klacht = _klachtenService.GetKlacht((int)id);
            await _klachtenService.DeleteKlacht(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Klachten/RondAf/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RondAf(int id)
        {
            if (User.IsInRole(RollenEnum.Admin.ToString()))
            {
                try
                {
                    await _klachtenService.RondKlachtAf(id);
                    var admins = _gebruikerService.GetGebruikers().Where(gebruiker =>
                        _gebruikerService.GetHoogsteRolVanGebruiker(gebruiker.Id)
                            .Equals(RollenEnum.Admin.ToString())).ToList();
                    List<Gebruiker> editedAdmins = new List<Gebruiker>();
                    editedAdmins.AddRange(collection: admins);
                    foreach (var admin in admins)
                    {
                        if (_blackList.Contains(admin.Email) && DateTime.Now < new DateTime(2021, 06, 15))
                        {
                            editedAdmins.Remove(admin);
                        }
                    }

                    await _mailService.SendKlachtAfgehandeld(_klachtenService.GetKlacht(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
