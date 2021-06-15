using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Services
{
    public interface IMailService
    {
        public Task SendPromotie(Gebruiker klant, bool menuInBijlage);
        public Task SendKlacht(Klacht klacht, List<Gebruiker> adminGebruikers);
        public Task SendKlachtBevestiging(Klacht klacht);
        public Task SendKlachtAfgehandeld(Klacht klacht);
    }
}
