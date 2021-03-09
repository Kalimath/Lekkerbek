using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Models
{
    public class Bestelling
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Levertijd { get; set; }

        public string Opmerkingen { get; set; }

        public ICollection<Gerecht> GerechtenLijst { get; set; }
        public int AantalMaaltijden { get; set; }
        public int KlantId { get; set; }
        public virtual Klant Klant { get; set; }
        public DateTime? Tijdslot { get; set; }

        /*public double getTotaalPrijs(List<Gerecht> gerechten)
        {
            
            double totaalPrijs = 0;
                GerechtenLijst.AsQueryable().ForEachAsync(gerecht => totaalPrijs+=gerecht.Prijs);
                if (Klant.Bestellingen.Count >= 3)
                {
                    totaalPrijs *= 0.7;
                }

                return totaalPrijs;
        }*/
    }
}
