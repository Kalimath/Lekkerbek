using System;
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

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
    public class BestellingController : Controller
    {
        private readonly IdentityContext _context;
        private readonly IBestellingService _bestellingService;
        private readonly IGebruikerService _gebruikerService;
        private readonly IGerechtService _gerechtService;
        private readonly ICategorieService _categorieService;
        public BestellingController(IdentityContext context, IBestellingService bestellingService, IGebruikerService gebruikerService, IGerechtService gerechtService, ICategorieService categorieService)
        {
            _context = context;
            _bestellingService = bestellingService;
            _gebruikerService = gebruikerService;
            _gerechtService = gerechtService;
            _categorieService = categorieService;
        }

        // GET: Bestelling
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = _gebruikerService.GetHuidigeGebruiker();
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
            ViewData["Klanten"] = new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName");
            ViewData["AlleGerechtenNamen"] = new SelectList(_gerechtService.GetGerechten(), "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten().Result.Where(tijdslot => tijdslot.IsVrij), "Tijdstip", "Tijdstip");
            return View();
        }

        // POST: Bestelling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                var tijdslot = _context.AlleVrijeTijdsloten()
                    .Result.Find(tijdslot => tijdslot.Tijdstip == DateTime.Parse(collection["Tijdslot"]));
                tijdslot.IsVrij = false;
                Gebruiker klantVanBestelling = _gebruikerService.GetHuidigeGebruiker();
                Bestelling bestelling = new Bestelling()
                {
                    AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]),
                    GerechtenLijst = new List<Gerecht>(),
                    Opmerkingen = collection["Opmerkingen"],
                    Levertijd = DateTime.Parse(collection["Levertijd"]),                    
                    Tijdslot = tijdslot
                };
                IEnumerable<string> gerechtNamen = (ICollection<string>)collection["GerechtenLijst"];
                bestelling.GerechtenLijst = await _gerechtService.GetGerechten().AsQueryable().Where(gerecht => gerechtNamen.Contains(gerecht.Naam)).ToListAsync();
                
                if (User.IsInRole(RollenEnum.Klant.ToString()))
                {
                    bestelling.KlantId = klantVanBestelling.Id;
                }
                else
                {
                    bestelling.KlantId = int.Parse(collection["Klant"]); 
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
            ViewData["Naam"] = new SelectList(_gerechtService.GetGerechten(), "Naam", "Naam");
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> VoegGerechtenToe(int id, IFormCollection collection)
        {
            var gerecht = _gerechtService.GetGerecht(collection["Naam"]);
            Bestelling bestelling = _bestellingService.GetBestelling(id);
            try
            {
                await _bestellingService.AddGerechtAanBestelling(id, gerecht);
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
            var huidigtijdslot = _context.Tijdslot.Find(tijdslotId); 
            var tijdsloten = _context.AlleVrijeTijdsloten();
            ViewData["HuidigTijdslot"] = huidigtijdslot.Tijdstip; 
            ViewData["Tijdslot"] = new SelectList(tijdsloten.Result, "Id", "Tijdstip"); 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> WijzigTijdslot(int id, int tijdslotid, IFormCollection collection)
        {
            var huidigtijdslot = _context.Tijdslot.Find(tijdslotid); 
            var tijdsloten = _context.AlleVrijeTijdsloten();

            var nieuwTijdslotId = int.Parse(collection["Tijdslot"]); 
            var nieuwTijdSlot = _context.AlleVrijeTijdsloten().Result.Find(nt => nt.Id == nieuwTijdslotId);

            var bestelling = _bestellingService.GetBestelling(id);

            huidigtijdslot.IsVrij = true;
            nieuwTijdSlot.IsVrij = false; 
            bestelling.Tijdslot = nieuwTijdSlot;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Bestelling/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

            ViewData["HuidigeKlant"] = _gebruikerService.GetGebruikerMetRolKlant(bestelling.KlantId); 
            ViewData["Klanten"] = new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName");
            ViewData["AlleGerechtenNamen"] = new SelectList(_gerechtService.GetGerechten(), "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten().Result, "Tijdstip", "Tijdstip");
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

                    bestelling = _bestellingService.GetBestelling(id);
                    bestelling.AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]);
                    bestelling.Opmerkingen = collection["Opmerkingen"];
                    bestelling.Levertijd = DateTime.Parse(collection["Levertijd"]);

                    if (User.IsInRole(RollenEnum.Admin.ToString()))
                    { 
                        Gebruiker klantVanBestelling =  _gebruikerService.GetGebruikerMetRolKlant(int.Parse(collection["Klant"]));
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
            ViewData["AlleGerechtenNamen"] = new SelectList(_gerechtService.GetGerechten(), "Naam", "Naam");
            ViewData["Tijdslot"] = new SelectList(_context.AlleVrijeTijdsloten().Result, "Tijdstip", "Tijdstip");
            return View(bestelling);
        }

        // GET: Bestelling/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = _bestellingService.GetAlleBestellingen().Result.First(bestelling1 => bestelling1.Id == id);
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
            if (bestelling.Tijdslot != _bestellingService.GetAlleBestellingen().Result.Find(bestelling1 => bestelling1.Id == id).Tijdslot)
            {
                _context.Tijdslot.Include("InGebruikDoorKok").ToList().Find(tijdslot => tijdslot.Tijdstip == _bestellingService.GetBestelling(id).Tijdslot.Tijdstip).IsVrij = true;
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

        // POST: Bestelling/Delete/5
        [HttpPost, ActionName("Afronden")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> BestellingAfronden(int id)
        {
            Bestelling bestelling = _bestellingService.GetBestelling(id);
            bestelling.Tijdslot = await _context.Tijdslot.Include(tijdslot => tijdslot.InGebruikDoorKok)
                .FirstAsync(tijdslot => tijdslot.Id == bestelling.Tijdslot.Id);
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


        public JsonResult LaadAlleBestellingen()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = _gebruikerService.GetHuidigeGebruiker();
                return Json(new {data = _bestellingService.GetBestellingenVanKlant(currentUser.Id)});
            }
            else
            {
                return Json(new { data = _bestellingService.GetAlleBestellingen().Result});
            }
        }
    }
}
