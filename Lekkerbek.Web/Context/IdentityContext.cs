using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Context
{
    public class IdentityContext : IdentityDbContext<Gebruiker, Role, int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
        public DbSet<Bestelling> Bestellingen { get; set; }

        public DbSet<Kok> Koks { get; set; }
        public DbSet<Gerecht> Gerechten { get; set; }
        public DbSet<Lekkerbek.Web.Models.Gerecht> Gerecht { get; set; }
        public DbSet<Lekkerbek.Web.Models.Categorie> Categorie { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Role> Rollen { get; set; }
        public List<Gebruiker> GebruikersMetRolKlant()
        {
            var klanten = from u in Users join r in UserRoles on u.Id equals r.UserId where r.RoleId == 3 select u;
            return klanten.ToList();
        }
        public List<string> KlantNamen()
        {
            List<string> klantNamen = new List<string>();
            GebruikersMetRolKlant().ToList().ForEach(klant => klantNamen.Add(klant.UserName));
            return klantNamen;
        }

        public List<Tijdslot> Alletijdsloten()
        {
            var sloten = new List<Tijdslot>();
            Koks.ForEachAsync(kok => sloten.AddRange(kok.Tijdsloten));
            return sloten;
        }
        public List<Tijdslot> AlleVrijeTijdsloten()
        {
            var sloten = new List<Tijdslot>();
            Koks.ForEachAsync(kok => sloten.AddRange(kok.Tijdsloten.Where(tijdslot => tijdslot.IsVrij)));
            return sloten;
        }

        public List<Bestelling> OpenstaandeBestellingenVanKlantMetId(int klantId)
        {
            return Bestellingen.AsQueryable().Where(bestelling => GebruikersMetRolKlant().Select(gebruiker => gebruiker.Id).Any(i => bestelling.KlantId == i))
                .Where(bestelling => !bestelling.IsAfgerond).ToList();
        }

        public ICollection<Gerecht> VoorkeursGerechtenVanKlanten(int klantId)
        {

            return Gebruikers.Find(klantId).Voorkeursgerechten;
        }
        //geeft rollen terug van gebruiker met id
        public List<String> GebruikerRollen(int userId)
        {
            var rollen = from u in Users
                join ur in UserRoles on u.Id equals ur.UserId where u.Id == userId
                join r in Rollen on ur.RoleId equals r.Id
                select r.Name;

            return rollen.ToList();
        }

        //geeft voor iedere gebruiker de rol terug met de meeste rechten
        public Dictionary<int, string> HoogsteRollenGebruikers()
        {
            Dictionary<int, string> rollenGebruikers = new Dictionary<int, string>();
            foreach (var gebruiker in Gebruikers)
            {
                rollenGebruikers[gebruiker.Id] = GebruikerHoogsteRol(gebruiker.Id);
            }
            return rollenGebruikers;
        }

        //geeft de rol terug met de meeste rechten van gebruiker met id
        public String GebruikerHoogsteRol(int userId)
        {
            List<string> rollenGebruiker = GebruikerRollen(userId);
            if (rollenGebruiker.Contains(RollenEnum.Admin.ToString()))
            {
                return RollenEnum.Admin.ToString();
            }
            else if (rollenGebruiker.Contains(RollenEnum.Kassamedewerker.ToString()))
            {
                return RollenEnum.Kassamedewerker.ToString();
            }
            else if (rollenGebruiker.Contains(RollenEnum.Kok.ToString()))
            {
                return RollenEnum.Kok.ToString();
            }
            else
            {
                return RollenEnum.Klant.ToString();
            }
        }

    }
}
