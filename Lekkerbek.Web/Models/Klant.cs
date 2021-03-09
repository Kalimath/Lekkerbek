using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Klant
    {
        public Klant()
        {
            Bestellingen= new HashSet<Bestelling>();
        }
        private DateTime _geboortedatum;
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geboortedatum
        {
            get => _geboortedatum.Date;
            set => _geboortedatum = value;
        }

        public int Getrouwheidsscore { get; set; }
        public virtual ICollection<Gerecht> Voorkeursgerechten { get; set; } = new List<Gerecht>();
        public virtual ICollection<Bestelling> Bestellingen { get; set; }

    }
}
