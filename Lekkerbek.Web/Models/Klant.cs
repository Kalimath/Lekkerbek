using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models
{
    public class Klant : Gebruiker
    {
        public Klant()
        {
            Bestellingen = new HashSet<Bestelling>();
        }

        public int Getrouwheidsscore { get; set; }
        public virtual ICollection<Gerecht> Voorkeursgerechten { get; set; } = new List<Gerecht>();
        public virtual ICollection<Bestelling> Bestellingen { get; set; }

    }
}
