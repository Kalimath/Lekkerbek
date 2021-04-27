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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
    public class BestellingController : Controller
    {
        private readonly IdentityContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;
        public BestellingController(IdentityContext context, RoleManager<Role> roleManager, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Bestelling
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RollenEnum.Klant.ToString()))
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                return View(OpenstaandeBestellingenVanKlantMetId(currentUser.Id));
            }
            else
            {
                return View(AlleBestellingen().Result);
            }
        }

        // GET: Bestelling/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (this.BestellingExists((int) id))
            {
                Bestelling bestelling = await _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst").FirstAsync(bestelling1 => bestelling1.Id == id);
                bestelling.Tijdslot = await _context.Tijdslot.Include(tijdslot => tijdslot.InGebruikDoorKok)
                    .FirstAsync(tijdslot => tijdslot.Id == bestelling.Tijdslot.Id);
                ViewBag.TotaalPrijs = GerechtenTotaalPrijsAsync(bestelling, false).Result;
                ViewBag.TotaalPrijsInclBtw = GerechtenTotaalPrijsAsync(bestelling, true).Result;
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
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Id", "UserName");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie"), "Naam", "Naam");
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
                Gebruiker klantVanBestelling = await _userManager.GetUserAsync(User);
                Bestelling bestelling = new Bestelling()
                {
                    AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]),
                    GerechtenLijst = new List<Gerecht>(),
                    Opmerkingen = collection["Opmerkingen"],
                    Levertijd = DateTime.Parse(collection["Levertijd"]),                    
                    Tijdslot = tijdslot
                };
                IEnumerable<string> gerechtNamen = (ICollection<string>)collection["GerechtenLijst"];
                bestelling.GerechtenLijst = await _context.Gerechten.Where(gerecht => gerechtNamen.Contains(gerecht.Naam)).ToListAsync();
                
                if (User.IsInRole(RollenEnum.Klant.ToString()))
                {
                    bestelling.KlantId = klantVanBestelling.Id;
                }
                else
                {
                    bestelling.KlantId = int.Parse(collection["Klant"]); 
                }
                
                _context.Bestellingen.Add(bestelling);
                await _context.SaveChangesAsync();
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
            ViewData["Naam"] = new SelectList(_context.Gerechten.ToList(), "Naam", "Naam");
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> VoegGerechtenToe(int id, IFormCollection collection)
        {
            var gerecht = await _context.Gerechten.FirstAsync(g => g.Naam.Equals(collection["Naam"])); 
            var bestelling = await _context.Bestellingen.FindAsync(id);
            if(bestelling.GerechtenLijst == null)
            {
                bestelling.GerechtenLijst = new List<Gerecht>(); 
            }
            bestelling.GerechtenLijst.Add(gerecht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> VerwijderEenGerecht(String id, int bestellingId)
        {
            var gerecht = _context.Gerechten.Include("Categorie").First(g => g.Naam.Equals(id));
            var bestelling = _context.Bestellingen.Include("GerechtenLijst").ToList().First(b => b.Id == bestellingId);
            ViewData["bestellingId"] = bestellingId; 
            return View(gerecht);
        }
        [HttpPost]
        public async Task<IActionResult> VerwijderEenGerecht(String id, int bestellingid, IFormCollection collection)
        {
            var bestelling = _context.Bestellingen.Include("GerechtenLijst").ToList().First(b => b.Id == bestellingid);
            var gerecht = _context.Gerechten.Include("Categorie").First(g => g.Naam.Equals(id));
            bestelling.GerechtenLijst.Remove(gerecht);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public async Task<IActionResult> WijzigTijdslot(int id, int tijdslotId)
        {
            var huidigtijdslot = _context.Alletijdsloten().Find(t => t.Id == tijdslotId); 
            var tijdsloten = _context.AlleVrijeTijdsloten();
            var bestelling = _context.Bestellingen.Include("Tijdslot").ToList().Find(b => b.Id == id);
            ViewData["HuidigTijdslot"] = huidigtijdslot.Tijdstip; 
            ViewData["Tijdslot"] = new SelectList(tijdsloten, "Id", "Tijdstip"); 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> WijzigTijdslot(int id, int tijdslotid, IFormCollection collection)
        {
            var huidigtijdslot = _context.Alletijdsloten().Find(t => t.Id == tijdslotid); 
            var tijdsloten = _context.AlleVrijeTijdsloten();

            var nieuwTijdslotId = int.Parse(collection["Tijdslot"]); 
            var nieuwTijdSlot = _context.AlleVrijeTijdsloten().Find(nt => nt.Id == nieuwTijdslotId); 

            var bestelling = _context.Bestellingen.Include("Tijdslot").ToList().Find(b => b.Id == id);

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


            //var bestelling = _context.Bestellingen.Include("Tijdslot").ToList().Find(g => g.Id == id);
            var bestelling = await _context.Bestellingen
                .Include(b => b.Tijdslot)
                .Include(b => b.GerechtenLijst)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestelling == null)
            {
                return NotFound();
            }
            /**
             * In comment want View-site beperking => in geval admin iets met bestellingen wilt doen 
             * gaat niet vanwege beperking
             */
            /*if (DateTime.Now > bestelling.Tijdslot.Tijdstip.AddHours(-1))
            {
                return RedirectToAction(nameof(Index));
            }*/

            _context.Tijdslot.Include("InGebruikDoorKok").ToList().Find(tijdslot => tijdslot.Tijdstip == bestelling.Tijdslot.Tijdstip).IsVrij = true;
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie"), "Naam", "Naam");
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
                    Gebruiker klantVanBestelling = await _context.GebruikersMetRolKlant().AsQueryable().FirstAsync(klant => klant.UserName.Trim().ToLower().Equals(collection["Klant.Naam"].ToString().Trim().ToLower()));
                    bestelling = _context.Bestellingen.First(bestelling => bestelling.Id == id);
                    bestelling.AantalMaaltijden = Int32.Parse(collection["AantalMaaltijden"]);
                    bestelling.Klant = currentUser;
                    bestelling.Opmerkingen = collection["Opmerkingen"];
                    bestelling.Levertijd = DateTime.Parse(collection["Levertijd"]);
                    bestelling.KlantId = currentUser.Id;
                    //bestelling.Tijdslot = new Tijdslot(DateTime.Parse(collection["Tijdslot"]));

                    //Maak tijdslot weer beschikbaar als dit is veranderd of bestelling is verwijderd.
                    /*if (collection["Tijdslot"] != bestelling.Tijdslot)
                    {
                        _context.Alletijdsloten().ToList().Find(tijdslot => tijdslot.Tijdstip == _context.Bestellingen.Find(id).Tijdslot.Tijdstip).IsVrij = true;
                    }*/
                    //_context.Alletijdsloten().First(tijdslot => tijdslot.Tijdstip == DateTime.Parse(collection["Tijdslot"]) && tijdslot.IsVrij).IsVrij = false;
                    //IEnumerable<string> gerechtNamen = (ICollection<string>)collection["GerechtenLijst"];
                    /*var nieuweGerechten = _context.Gerechten.Where(gerecht => gerechtNamen.Contains(gerecht.Naam)).ToList()
                        .AsQueryable();*/
                    
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Klanten"] = new SelectList(_context.GebruikersMetRolKlant(), "Naam", "Naam");
            ViewData["AlleGerechtenNamen"] = new SelectList(_context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie"), "Naam", "Naam");
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

            var bestelling = AlleBestellingen().Result.First(bestelling1 => bestelling1.Id == id);
            if (bestelling == null)
            {
                return NotFound();
            }
            /**
             * In comment want View-site beperking => in geval admin iets met bestellingen wilt doen 
             * gaat niet vanwege beperking
             */

            /*if (DateTime.Now > bestelling.Tijdslot.Tijdstip.AddHours(-2))
            {
                return RedirectToAction(nameof(Index)); 
            }*/
            return View(bestelling);

        }

        // POST: Bestelling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);

            //Maak tijdslot weer beschikbaar als dit is veranderd of bestelling is verwijderd.
            if (bestelling.Tijdslot != AlleBestellingen().Result.Find(bestelling1 => bestelling1.Id == id).Tijdslot)
            {
                _context.Tijdslot.Include("InGebruikDoorKok").ToList().Find(tijdslot => tijdslot.Tijdstip == _context.Bestellingen.Find(id).Tijdslot.Tijdstip).IsVrij = true;
            }

            _context.Bestellingen.Remove(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Bestelling/Delete/5
        [HttpPost, ActionName("Afronden")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker")]
        public async Task<IActionResult> BestellingAfronden(int id)
        {
            Bestelling bestelling = await _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst").FirstAsync(bestelling1 => bestelling1.Id == id);
            bestelling.Tijdslot = await _context.Tijdslot.Include(tijdslot => tijdslot.InGebruikDoorKok)
                .FirstAsync(tijdslot => tijdslot.Id == bestelling.Tijdslot.Id);
            if (bestelling.Tijdslot.InGebruikDoorKok != null)
            {
                bestelling.IsAfgerond = true;
            }
            _context.Bestellingen.Update(bestelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool BestellingExists(int id)
        {
            return AlleBestellingen().Result.Any(e => e.Id == id);
        }

        public async Task<List<Bestelling>> AlleBestellingen()
        {
            return await _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst").ToListAsync();
        }

        public List<Bestelling> OpenstaandeBestellingenVanKlantMetId(int klantId)
        {
            return _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst")
                .Where(bestelling => bestelling.KlantId == klantId).ToList();
        }

        private async Task<double> GerechtenTotaalPrijsAsync(Bestelling bestelling, bool isInclBtw)
        {
            var gerechten =  await _context.Gerechten.Include("Bestellingen").Include("VoorkeursgerechtenVanKlanten").Include("Categorie").AsQueryable().Where(gerecht =>
                gerecht.Bestellingen.Any(bestellingTemp => bestellingTemp.Id == bestelling.Id)).ToListAsync();
            double totaalPrijs = 0;
            if (isInclBtw)
            {
                foreach (var g in gerechten)
                {
                    totaalPrijs += g.PrijsInclBtw();
                }
                //gerechten.ForEachAsync(gerecht => totaalPrijs += gerecht.PrijsInclBtw());
            }
            else
            {
                foreach (var g in gerechten)
                {
                    totaalPrijs += g.Prijs;
                }
                //gerechten.ForEachAsync(gerecht => totaalPrijs += gerecht.Prijs);
            }

            /*var bestellingen = _context.GebruikersMetRolKlant().Find(gebruiker => gebruiker.Id == bestelling.KlantId).Bestellingen;*/
            int aantalBestellingen = await _context.Bestellingen
                .CountAsync(b => b.KlantId == bestelling.KlantId);
            if (aantalBestellingen >= 3)
            {
                totaalPrijs *= 0.9;
            }

            return Math.Round(totaalPrijs, 2); ;
        }

        /*public async Task<IActionResult> MijnBestellingen(int? klantId)
        {
            var klanten = _context.GebruikersMetRolKlant().AsQueryable();
            return View(klanten.First((klant => klant.Id == klantId)).Bestellingen);
        }*/
    }
}
