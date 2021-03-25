using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Lekkerbek.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;

        public AccountController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterForm()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(IFormCollection collection)
        {
            try
            {
                Gebruiker gebruiker = await _userManager.FindByEmailAsync(collection["Email"]);

                if (gebruiker == null)
                {
                    gebruiker = new Gebruiker()
                    {
                        Email = collection["Email"],
                        Naam = collection["Naam"],
                        Geboortedatum = DateTime.Parse(collection["Geboortedatum"]),
                        PasswordHash = collection["PasswordHash"],
                        Adres = collection["Adres"]
                    };
                    _userManager.AddToRoleAsync(gebruiker, RollenEnum.Klant.ToString());
                    _userManager.UpdateAsync(gebruiker);

                }
                else
                {
                    throw new  Exception("Klant met email " + collection["Email"] + " bestaat al.");
                }

                _signInManager.SignInAsync(gebruiker,false,null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return RedirectToAction("index");
        }


        public IActionResult Index()
        {
            
            return View();
        }

        public JsonResult LoadAllUsers()
        {
            return Json(_userManager.Users);
        }
    }
}
