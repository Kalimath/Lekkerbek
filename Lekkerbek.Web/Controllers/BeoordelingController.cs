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
using Microsoft.EntityFrameworkCore;

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
            List<BeoordelingMetKlantNaamViewModel> viewModels =  new List<BeoordelingMetKlantNaamViewModel>();
            foreach (var beoordeling in _beoordelingService.GetBeoordelingen())
            {
                var username = _gebruikerService.GetGebruiker(beoordeling.KlantId).UserName;
               viewModels.Add( new BeoordelingMetKlantNaamViewModel(beoordeling.Id, beoordeling.Titel, beoordeling.Commentaar, beoordeling.ScoreLijst,beoordeling.KlantId, username));
            }
            return View(viewModels);
        }

        // GET: Beoordeling/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Beoordeling beoordeling = _beoordelingService.GetBeoordeling(id);
                BeoordelingMetKlantNaamViewModel viewModel = new BeoordelingMetKlantNaamViewModel(beoordeling.Id, beoordeling.Titel, beoordeling.Commentaar, beoordeling.ScoreLijst, beoordeling.KlantId, 
                                        _gebruikerService.GetGebruiker(beoordeling.KlantId).UserName);
                return View(viewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Create([Bind("Titel, Commentaar, ScoreLijst")] Beoordeling beoordeling)
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
        public IActionResult Edit(int id)
        {
            return View(_beoordelingService.GetBeoordeling(id));
        }

        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public async Task<IActionResult> Edit([Bind("Titel, Commentaar, ScoreLijst, KlantId")] Beoordeling beoordeling)
        {
            RedirectToActionResult result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    Beoordeling updatedBeoordeling = _beoordelingService.GetBeoordeling(beoordeling.Id);
                    updatedBeoordeling.Commentaar = beoordeling.Commentaar.Trim();
                    updatedBeoordeling.ScoreLijst = beoordeling.ScoreLijst;
                    updatedBeoordeling.DefineTotalScore();
                    await _beoordelingService.UpdateBeoordeling(updatedBeoordeling);
                    result = RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                     result = RedirectToAction("Edit",new{beoordeling.Id});
                }
                
            }
            return result;
        }

        public async Task<IActionResult> MijnBeoordelingen()
        {
            return View(_beoordelingService.GetBeoordelingenVanKlant((await _userManager.GetUserAsync(HttpContext.User)).Id));
        }

        // GET: Beoordeling/Delete/5
        
        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public async Task<IActionResult> Delete(int id)
        {
            Beoordeling beoordelingToDelete = null;
            try
            {
                beoordelingToDelete = _beoordelingService.GetBeoordeling(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (beoordelingToDelete == null)
            {
                return NotFound();
            }
            return View(beoordelingToDelete);
        }

        // POST: Beoordeling/Delete/5
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Kassamedewerker,Klant")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            try
            {
                if (User.IsInRole(RollenEnum.Admin.ToString()) || User.IsInRole(RollenEnum.Kassamedewerker.ToString())|| 
                    _beoordelingService.GetBeoordelingenVanKlant((await _userManager.GetUserAsync(HttpContext.User)).Id).Any(beoordeling => beoordeling.Id == id))
                {
                    await _beoordelingService.DeleteBeoordeling(id);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
