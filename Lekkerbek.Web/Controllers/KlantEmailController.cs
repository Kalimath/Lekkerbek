using Lekkerbek.Web.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.IO;
using System.Net;
using Lekkerbek.Web.Services;
using Lekkerbek.Web.ViewModels.EmailKlant;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KlantEmailController : Controller
    {
        private readonly IGebruikerService _gebruikerService;
        private readonly IMailService _mailService;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;

        public KlantEmailController(IdentityContext context, RoleManager<Role> roleManager, UserManager<Gebruiker> userManager, IGebruikerService gebruikerService, IMailService mailService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _gebruikerService = gebruikerService;
            _mailService = mailService;
        }
        
        public IActionResult Index()
        {
            ViewBag.Klanten = new SelectList(_gebruikerService.GetGebruikersMetRolKlant(), "Id", "UserName");
            return View();
        }


        public async Task<IActionResult> Send([Bind("Klant", "Pdf")] IndexViewModel vm)
        {
            try
            {

                Gebruiker klant = _gebruikerService.GetGebruiker(vm.Klant);
                await _mailService.SendPromotie(klant, vm.Pdf);

                return View(klant);
            }
            catch (Exception e)
            {
                return RedirectToAction("Exception", e); 
            }
        }

        public IActionResult Exception(Exception e)
        {
            return View(e.Message); 
        }
    }
}
