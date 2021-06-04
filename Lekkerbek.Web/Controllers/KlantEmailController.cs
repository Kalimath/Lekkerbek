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
using Lekkerbek.Web.ViewModels.EmailKlant;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KlantEmailController : Controller
    {
        private readonly IdentityContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<Gebruiker> _userManager;

        public KlantEmailController(IdentityContext context, RoleManager<Role> roleManager, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            ViewBag.Klanten = new SelectList(_context.GebruikersMetRolKlant(), "Id", "UserName");
            return View();
        }


        public async Task<IActionResult> Send([Bind("Klant", "Pdf")] IndexViewModel vm)
        {
            try
            {

                Gebruiker Klant = _context.GebruikersMetRolKlant().Find(g => g.Id ==  vm.Klant); 
                var message = new MailMessage();
                message.To.Add(new MailAddress(Klant.Email));
                message.From = new MailAddress("gip.sender.team11@outlook.com");
                message.Subject = "Lekkerbek Promotie";
                string textEmail = System.IO.File.ReadAllText("ExterneBestanden/EmailSjabloon.txt");
                string korting = ""; 
                string voorkeursgerechten = "";
                string aanspreking = ""; 
                if(Klant.Bestellingen.Count % 2 == 0 && Klant.Bestellingen.Count != 0 && Klant.Bestellingen.Count != 1)
                {
                    korting = "Op uw eerstvolgende bestelling ontvangt u een korting"; 
                }
                switch (Klant.Geslacht)
                {
                    case "Man":
                        aanspreking = "heer "; 
                        break;
                    case "Vrouw":
                        aanspreking = "mevrouw "; 
                            break;
                    default:
                        aanspreking = "";
                        break; 
                }
                foreach(Gerecht g in Klant.Voorkeursgerechten)
                {
                    voorkeursgerechten += "<p> - " + g.Naam + "</p>\n";
                }
                    
                textEmail = string.Format(textEmail, aanspreking,Klant.UserName, korting, voorkeursgerechten);
                message.Body = textEmail;
                message.IsBodyHtml = true;
                if (vm.Pdf)
                {
                    message.Attachments.Add(new Attachment("ExterneBestanden/MenukaartLekkerbek.pdf")); 
                }

                var smtp = new SmtpClient("smtp.outlook.com");
                smtp.Credentials = new NetworkCredential("gip.sender.team11@outlook.com", "Lekkerbek123");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

                return View(Klant);
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
