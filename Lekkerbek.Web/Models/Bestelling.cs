using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
