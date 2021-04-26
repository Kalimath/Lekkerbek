using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Models
{
    public class Bestelling
    {
        private double _totaalPrijs;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }
        public DateTime Levertijd { get; set; }
        public string Opmerkingen { get; set; }
        public ICollection<Gerecht> GerechtenLijst { get; set; }
        public int AantalMaaltijden { get; set; }
        public int KlantId { get; set; }
        public virtual Gebruiker Klant { get; set; }
        public Tijdslot Tijdslot { get; set; }
        public bool IsAfgerond { get; set; } = false;
        public bool IsAfhaling { get; set; } = false;

        /*public double TotaalPrijs
        {
            get => _totaalPrijs;
            set => _totaalPrijs = GerechtenTotaalPrijs(false);
        }

        public double TotaalPrijsInclBtw
        {
            get => _totaalPrijs;
            set => _totaalPrijs = GerechtenTotaalPrijs(true);
        }


        private double GerechtenTotaalPrijs(bool isInclBtw)
        {
            
            double totaalPrijs = 0;
            if (isInclBtw)
            {
                GerechtenLijst.AsQueryable().ForEachAsync(gerecht => totaalPrijs += gerecht.PrijsInclBtw);
            }
            else
            {
                GerechtenLijst.AsQueryable().ForEachAsync(gerecht => totaalPrijs += gerecht.Prijs);
            }
            if (Klant.Bestellingen.Count >= 3)
            {
                totaalPrijs *= 0.9;
            }

            return Math.Round(totaalPrijs, 2); ;
        }*/
    }
}
