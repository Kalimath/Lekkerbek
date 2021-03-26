using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly IdentityContext _context;

        public AccountController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, IdentityContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                        PasswordHash = HashPassword(collection["PasswordHash"]),
                        Adres = collection["Adres"]
                    };
                    _userManager.AddToRoleAsync(gebruiker, RollenEnum.Klant.ToString());
                    _context.Add(gebruiker);
                    await _context.SaveChangesAsync();
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
            return Json(new { data = _userManager.Users });
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return buffer3.Equals(buffer4);
        }
    }
}
