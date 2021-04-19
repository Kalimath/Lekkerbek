using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lekkerbek.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;

        public IndexModel(
            UserManager<Gebruiker> userManager,
            SignInManager<Gebruiker> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Geboortedatum { get; set; }
        public int Getrouwheidsscore { get; set; }
        public bool IsProfessional { get; set; }
        public string BtwNummer { get; set; }
        public string FirmaNaam { get; set; }
        public string Adres { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Nieuw adres")]
            public string Adres { get; set; }
        }

        private async Task LoadAsync(Gebruiker user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var nieuwAdres = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Adres = nieuwAdres
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            this.UserId = user.Id;
            this.Username = user.UserName;
            this.Email = user.Email;
            this.Getrouwheidsscore = user.Getrouwheidsscore;
            this.Geboortedatum = user.Geboortedatum;
            this.IsProfessional = user.IsProfessional;
            this.BtwNummer = user.BtwNummer;
            this.FirmaNaam = user.FirmaNaam;
            this.Adres = user.Adres;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (!Input.Adres.Equals(user.Adres))
            {
                user.Adres = Input.Adres;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
