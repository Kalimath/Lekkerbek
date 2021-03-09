using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Context
{
    public class BestellingDbContext:DbContext
    {
        public BestellingDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Klant> Klanten { get; set; }

        public List<string> KlantNamen()
        {
            List<string> klantNamen = new List<string>();
            Klanten.ToList().ForEach(klant => klantNamen.Add(klant.Naam));
            return klantNamen;
        }

        public DbSet<Tijdslot> Tijdsloten { get; set; }
        public DbSet<Gerecht> Gerechten { get; set; }
        public DbSet<Lekkerbek.Web.Models.Gerecht> Gerecht { get; set; }
        public DbSet<Lekkerbek.Web.Models.Categorie> Categorie { get; set; }

    }
}
