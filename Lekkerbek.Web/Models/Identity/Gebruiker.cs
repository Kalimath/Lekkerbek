using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lekkerbek.Web.Models.Identity
{
    public class Gebruiker : IdentityUser<int>
    {
        public Gebruiker(){
            Bestellingen = new HashSet<Bestelling>();
            Voorkeursgerechten = new HashSet<Gerecht>();
        }
        private DateTime _geboortedatum;
        public string Adres { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geboortedatum
        {
            get => _geboortedatum.Date;
            set => _geboortedatum = value;
        }

        public int Getrouwheidsscore { get; set; } = 0;
        public virtual ICollection<Gerecht> Voorkeursgerechten { get; set; }
        public virtual ICollection<Bestelling> Bestellingen { get; set; }

        [Required]
        [DisplayName("Professionele gebruiker")]
        public bool IsProfessional { get; set; }

        [DisplayName("Btw-nummer")]
        public string BtwNummer { get; set; }
        [DisplayName("Firma naam")]
        public string FirmaNaam { get; set; }
    }
    
}
