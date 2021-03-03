using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Bestelling
    {
        private DateTime _Leverdatum;
        public int Id { get; set; }

        public DateTime Leverdatum
        {
            get => _Leverdatum.Date;
            set => _Leverdatum = value;
        }

        public ICollection<Gerecht> GerechtenLijst { get; set; }
        public int AantalMaaltijden { get; set; }
        public int KlantId { get; set; }
        public virtual Klant Klant { get; set; }
    }
}
