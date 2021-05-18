using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Context
{
    public class IdentityContext : IdentityDbContext<Gebruiker, Role, int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
        public DbSet<Bestelling> Bestellingen { get; set; }

        /*public DbSet<Agenda> Agenda { get; set; }*/
        public DbSet<Gerecht> Gerechten { get; set; }
        public DbSet<Lekkerbek.Web.Models.Gerecht> Gerecht { get; set; }
        public DbSet<Lekkerbek.Web.Models.Categorie> Categorie { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Role> Rollen { get; set; }
        public List<Gebruiker> GebruikersMetRolKlant()
        {
            var klanten = from u in Gebruikers.Include("Bestellingen").Include("Voorkeursgerechten") join r in UserRoles on u.Id equals r.UserId where r.RoleId == 3 select u;
            return klanten.ToList();
        }
        public List<string> KlantNamen()
        {
            List<string> klantNamen = new List<string>();
            GebruikersMetRolKlant().ToList().ForEach(klant => klantNamen.Add(klant.UserName));
            return klantNamen;
        }

        public async Task<List<Tijdslot>> AlleVrijeTijdsloten()
        {
            return await Tijdslot.Include("InGebruikDoorKok").Where(tijdslot => tijdslot.IsVrij).ToListAsync();
        }

        public ICollection<Gerecht> VoorkeursGerechtenVanKlanten(int klantId)
        {
            return Gerechten.Include("VoorkeursgerechtenVanKlanten").Include("Categorie").Where(gerecht => gerecht.VoorkeursgerechtenVanKlanten.Any(gebruiker => gebruiker.Id == klantId)).ToList();
        }

        //geeft rollen terug van gebruiker met id
        public List<String> GebruikerRollen(int userId)
        {
            var rollen = from u in Gebruikers.Include("Bestellingen").Include("Voorkeursgerechten")
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

        public DbSet<Lekkerbek.Web.Models.Tijdslot> Tijdslot { get; set; }


        public async Task<List<Tijdslot>> TijdslotenToegankelijkVoorKok(Gebruiker kok)
        {
            var tijdsloten = new List<Tijdslot>();

            //alle tijdsloten die aan kok zijn toegewezen
            List<Tijdslot> list = await Tijdslot.Include("InGebruikDoorKok").Where(tijdslot => tijdslot.InGebruikDoorKok!=null && tijdslot.InGebruikDoorKok.Id == kok.Id).ToListAsync();

            //alle tijdsloten waarvan de bestelling nog niet is afgerond
            IQueryable<Tijdslot> tijdsloten2 = from bestelling in Bestellingen.Include("Tijdslot")
                join tijdslot in Tijdslot.Include("InGebruikDoorKok") on bestelling.Tijdslot.Id equals tijdslot.Id 
                where (bestelling.IsAfgerond == false && list.Contains(tijdslot)&&tijdslot.InGebruikDoorKok==kok)
                select tijdslot;

            if (tijdsloten2.Count()>0)
            {
                tijdsloten = tijdsloten2.ToList();
            }
            else
            {
                tijdsloten = await Tijdslot.Include("InGebruikDoorKok").Where(tijdslot => tijdslot.InGebruikDoorKok == null).ToListAsync();
            }
            
            return tijdsloten;
        }
    }
}
