using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Lekkerbek.Web.Services;
using Lekkerbek.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Lekkerbek.Web.Controllers
{
    [AllowAnonymous]
    public class BeoordelingController : Controller
    {
        
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly IGebruikerService _gebruikerService;
        private readonly IBeoordelingService _beoordelingService;
        public BeoordelingController(RoleManager<Role> roleManager, UserManager<Gebruiker> userManager,IGebruikerService iGebruikerService, IBeoordelingService iBeoordelingService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _gebruikerService = iGebruikerService;
            _beoordelingService = iBeoordelingService;
        }
        public IActionResult Index()
        {
            return View(_beoordelingService.GetBeoordelingen());
        }

        // GET: Beoordeling/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var beoordeling = _beoordelingService.GetBeoordeling(id);
                BeoordelingMetKlantNaamViewModel viewModel = new BeoordelingMetKlantNaamViewModel(beoordeling.Commentaar, beoordeling.ScoreLijst, beoordeling.KlantId, 
                                        _gebruikerService.GetGebruiker(beoordeling.KlantId).UserName);
                return View(viewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(nameof(Index));
            }
            
        }


        [Authorize(Roles = "Klant")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klant")]
        public async Task<IActionResult> Create([Bind("Commentaar,ScoreLijst")] Beoordeling beoordeling)
        {
            if (ModelState.IsValid)
            {
                if (_gebruikerService.GetHoogsteRolVanGebruiker((await _userManager.GetUserAsync(HttpContext.User)).Id).Equals(RollenEnum.Klant.ToString()))
                {
                    var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
                    beoordeling.KlantId = userId;

                    await _beoordelingService.AddBeoordeling(beoordeling);
                    return RedirectToAction(nameof(MijnBeoordelingen));
                }
                
            }
            return View(beoordeling);
        }

        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public IActionResult Delete(int id)
        {
            try
            {
                _beoordelingService.DeleteBeoordeling(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public IActionResult Edit(int id)
        {
            return View(_beoordelingService.GetBeoordeling(id));
        }

        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public async Task<IActionResult> Edit([Bind("Id,KlantId,Commentaar,EtenEnDrinkenScore,PrijsKwaliteitScore,ServiceScore,HygieneScore")] BeoordelingMetScoresViewModel beoordelingMetScores)
        {
            RedirectToActionResult result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var scores = new ScoreLijst()
                    {
                        EtenEnDrinkenScore = beoordelingMetScores.EtenEnDrinkenScore,
                        HygieneScore = beoordelingMetScores.HygieneScore,
                        PrijsKwaliteitScore = beoordelingMetScores.PrijsKwaliteitScore,
                        ServiceScore = beoordelingMetScores.ServiceScore
                    };
                    Beoordeling beoordeling = _beoordelingService.GetBeoordeling(beoordelingMetScores.Id);
                    beoordeling.Commentaar = beoordelingMetScores.Commentaar.Trim();
                    beoordeling.ScoreLijst = scores;
                    beoordeling.TotaalScore = beoordeling.TotaalScore;
                    await _beoordelingService.UpdateBeoordeling(beoordeling);
                    result = RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                     result = RedirectToAction("Edit",new{beoordelingMetScores.Id});
                }
                
            }
            return result;
        }

        public async Task<IActionResult> MijnBeoordelingen()
        {
            return View(_beoordelingService.GetBeoordelingenVanKlant((await _userManager.GetUserAsync(HttpContext.User)).Id));
        }
    }
}
