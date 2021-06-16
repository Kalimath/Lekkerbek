﻿using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Net.Mail;
using Lekkerbek.Web.ViewModels.Bestelling;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
    public class BestellingController : Controller
    {
        private readonly IBestellingService _bestellingService;
        private readonly IGebruikerService _gebruikerService;
        private readonly IGerechtService _gerechtService;
        private readonly ICategorieService _categorieService;
        private readonly ITijdslotService _tijdslotService; 
        private readonly UserManager<Gebruiker> _userManager;

        public BestellingController(IdentityContext context, IBestellingService bestellingService, IGebruikerService gebruikerService, IGerechtService gerechtService, ICategorieService categorieService, ITijdslotService tijdslotService,UserManager<Gebruiker> userManager)
        {
            _bestellingService = bestellingService;
            _gebruikerService = gebruikerService;
            _gerechtService = gerechtService;
            _categorieService = categorieService;
            _tijdslotService = tijdslotService; 
            _userManager = userManager;
        }

        // GET: Bestelling
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
                return View(_bestellingService.GetBestellingenVanKlant(currentUser.Id));
            }
            else
            {
                return View(await _bestellingService.GetAlleBestellingen());
            }
        }

        // GET: Bestelling/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_bestellingService.BestellingExists((int) id))
            {
                Bestelling bestelling = _bestellingService.GetBestelling((int)id);
                ViewBag.TotaalPrijs = _gerechtService.GerechtenTotaalPrijsAsync(bestelling, false);
                ViewBag.TotaalPrijsInclBtw = _gerechtService.GerechtenTotaalPrijsAsync(bestelling, true);
                return View(bestelling);
            }
            else
            {
                return NotFound();
            }
            
        }

        // GET: Bestelling/Create
        public IActionResult Create()
        {

            var klanten =  new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName");
            var gerechten = new SelectList(_gerechtService.GetGerechten(), "Naam", "Naam");
            var tijdslot = new SelectList(_tijdslotService.GetAlleTijdslotenZonderDuplicates(), "Tijdstip", "Tijdstip");

            var vm = new CreateViewModel() {
                KlantenLijst = klanten,
                GerechtenLijst = gerechten,
                Tijdsloten = tijdslot
            }; 
            //ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Id", "UserName");
            //ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie"), "Naam", "Naam");
            //ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten().Result.Where(tijdslot => tijdslot.IsVrij), "Tijdstip", "Tijdstip");
            return View(vm);
        }

        // POST: Bestelling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlantId", "Levertijd", "Opmerkingen", "AantalMaaltijden", "GerechtenNamen", "Tijdstip")]CreateViewModel vm)
        {
            try
            { 
                var tijdslot = _tijdslotService.GetVrijeTijdsloten()
                    .FirstOrDefault(tijdslot => tijdslot.Tijdstip == vm.Tijdstip);
                tijdslot.IsVrij = false;
                Gebruiker klantVanBestelling = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
                Bestelling bestelling = new Bestelling()
                {
                    AantalMaaltijden = vm.AantalMaaltijden,
                    GerechtenLijst = new List<Gerecht>(),
                    Opmerkingen = vm.Opmerkingen,
                    Tijdslot = tijdslot
                };
                IEnumerable<string> gerechtNamen = (ICollection<string>)vm.GerechtenNamen;
                List<Gerecht> list = (from gerecht1 in _gerechtService.GetGerechten().AsQueryable().Where(gerecht => gerechtNamen.Contains(gerecht.Naam))
                                      select gerecht1).ToList();
                bestelling.GerechtenLijst = list;
                
                if (User.IsInRole(RollenEnum.Klant.ToString()))
                {
                    bestelling.KlantId = klantVanBestelling.Id;
                }
                else
                {
                    bestelling.KlantId = vm.KlantId;
                }
                await _bestellingService.AddBestelling(bestelling);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult VoegGerechtenToe(int id)
        { 
            var gerechteKlantBestelling = _bestellingService.GetBestelling(id);
            List<Gerecht> gerechtenLijstKlant = new List<Gerecht>(); 
            foreach(Gerecht g in _gerechtService.GetGerechten())
            {
                if (!gerechteKlantBestelling.GerechtenLijst.Contains(g))
                {
                    gerechtenLijstKlant.Add(g); 
                }
            }

            VoegGerechtenToeViewModel vm = new VoegGerechtenToeViewModel()
            {
                Id = id,
                GerechtenLijst = new SelectList(gerechtenLijstKlant, "Naam", "Naam")
            };

            return View(vm); 
        }

        [HttpPost]
        public async Task<IActionResult> VoegGerechtenToe([Bind("Id", "ToeTeVoegenGerecht")]VoegGerechtenToeViewModel vm)
        {
            var gerecht = _gerechtService.GetGerecht(vm.ToeTeVoegenGerecht);
            Bestelling bestelling = _bestellingService.GetBestelling(vm.Id);
            try
            {
                await _bestellingService.AddGerechtAanBestelling(vm.Id, gerecht);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToPage("Error");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> VerwijderEenGerecht(String gerechtNaam, int bestellingId)
        {
            var gerecht = _gerechtService.GetGerecht(gerechtNaam);
            try
            {
                await _bestellingService.DeleteGerechtVanBestelling(gerechtNaam, bestellingId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToPage("Error");
            }
            return View(gerecht);
        }

        [HttpPost]
        public async Task<IActionResult> VerwijderEenGerecht(String gerechtNaam, int bestellingid, IFormCollection collection)
        {
            try
            {
                await _bestellingService.DeleteGerechtVanBestelling(gerechtNaam, bestellingid);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToPage("Error");
            }
            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public async Task<IActionResult> WijzigTijdslot(int id, int tijdslotId)
        {
            WijzigTijdslotViewModel vm = new WijzigTijdslotViewModel();

            vm.Id = id;
            vm.HuidigTijdslotId = tijdslotId;
            vm.HuidigTijdslot = _tijdslotService.GetTijdslot(tijdslotId);

            var tijdsloten = _tijdslotService.GetAlleTijdslotenZonderDuplicates();
            ViewData["Tijdslot"] = new SelectList(tijdsloten, "Id", "Tijdstip"); 
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> WijzigTijdslot(int tijdslotId, WijzigTijdslotViewModel vm)
        {
            var updatedTijdslot = _bestellingService.GetBestelling(vm.Id);
            updatedTijdslot.Tijdslot = _tijdslotService.GetTijdslot(vm.NieuwTijdslotId);
            await _bestellingService.UpdateBestelling(updatedTijdslot);
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Bestelling/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            EditViewModel vm = new EditViewModel(); 
            
            if (id == null)
            {
                return NotFound();
            }
            var bestelling = _bestellingService.GetBestelling((int) id);
            if (bestelling == null)
            {
                return NotFound();
            }

            //Check welke gebruiker request doet vervolgens nakijken of tijdslot verstreken is
            if (!(User.IsInRole(RollenEnum.Admin.ToString()) || User.IsInRole(RollenEnum.Kassamedewerker.ToString())))
            {
                if (DateTime.Now > bestelling.Tijdslot.Tijdstip.AddHours(-1))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            vm.bestelling = bestelling; 
            vm.HuidigeKlant = _gebruikerService.GetGebruikerMetRolKlant(bestelling.KlantId);
            vm.Klanten = new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName", vm.HuidigeKlant);
            vm.AlleGerechtNamen = _gerechtService.GetGerechten().ToList();
            vm.Tijdslot = new SelectList(_tijdslotService.GetAlleTijdslotenZonderDuplicates(), "Tijdstip", "Tijdstip"); 
           
            return View(vm);
        }

        // POST: Bestelling/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind ("KlantId", "AlleGerechtNamen", "TijdSlot", "bestelling")]EditViewModel vm)
        {
            Bestelling bestelling = null;
            

            if (ModelState.IsValid)
            {
                try
                {

                    bestelling = _bestellingService.GetBestelling(id); 
                    bestelling.AantalMaaltijden = vm.bestelling.AantalMaaltijden;
                    bestelling.Opmerkingen = vm.bestelling.Opmerkingen ;

                    if (User.IsInRole(RollenEnum.Admin.ToString()))
                    { 
                        Gebruiker klantVanBestelling =  _gebruikerService.GetGebruikerMetRolKlant(vm.KlantId);
                        bestelling.Klant = klantVanBestelling;
                        bestelling.KlantId = klantVanBestelling.Id;
                    }

                    try
                    {
                        await _bestellingService.UpdateBestelling(bestelling);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        RedirectToPage("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Klanten"] = new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName");
            return RedirectToAction(nameof(Edit), new{id});
        }

        // GET: Bestelling/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = (await _bestellingService.GetAlleBestellingen()).First(bestelling1 => bestelling1.Id == id);
            if (bestelling == null)
            {
                return NotFound();
            }
            //Check welke gebruiker request doet vervolgens nakijken of tijdslot verstreken is
            if (!(User.IsInRole(RollenEnum.Admin.ToString()) || User.IsInRole(RollenEnum.Kassamedewerker.ToString())))
            {
                if (DateTime.Now > bestelling.Tijdslot.Tijdstip.AddHours(-2))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(bestelling);

        }

        // POST: Bestelling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestelling = _bestellingService.GetBestelling(id);

            //Maak tijdslot weer beschikbaar als dit is veranderd of bestelling is verwijderd.
            if (bestelling.Tijdslot != (await _bestellingService.GetAlleBestellingen()).Find(bestelling1 => bestelling1.Id == id).Tijdslot)
            {
                _tijdslotService.GetAlleTijdsloten().FirstOrDefault(tijdslot => tijdslot.Tijdstip == _bestellingService.GetBestelling(id).Tijdslot.Tijdstip).IsVrij = true;
            }

            try
            {
                await _bestellingService.DeleteBestelling(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToPage("Error");
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Bestelling/BestellingAfronden/5
        [HttpPost, ActionName("Afronden")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> BestellingAfronden(int id)
        {
            Bestelling bestelling = _bestellingService.GetBestelling(id);
            
            if (bestelling.Tijdslot.InGebruikDoorKok != null)
            {
                bestelling.IsAfgerond = true;
            }
            try
            {
                await _bestellingService.UpdateBestelling(bestelling);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToPage("Error");
            }
            return RedirectToAction(nameof(Details), new { id });
        }


        public async Task<JsonResult> LaadAlleBestellingen()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = _gebruikerService.GetGebruikerInfo(await _userManager.GetUserAsync(HttpContext.User));
                return Json(new {data = _bestellingService.GetBestellingenVanKlant(currentUser.Id)});
            }
            else
            {
                return Json(new { data = _bestellingService.GetAlleBestellingen().Result});
            }
        }
    }
}
