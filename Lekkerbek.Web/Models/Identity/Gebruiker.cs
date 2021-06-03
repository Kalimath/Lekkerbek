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
        [Required]
        public string Adres { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Geboortedatum
        {
            get => _geboortedatum.Date;
            set => _geboortedatum = value;
        }

        public int Getrouwheidsscore { get; set; } = 0;
        public ICollection<Gerecht> Voorkeursgerechten { get; set; }
        public virtual ICollection<Bestelling> Bestellingen { get; set; }
        public virtual ICollection<Beoordeling> Beoordelingen { get; set; }

        [Required]
        [DisplayName("Professionele gebruiker")]
        public bool IsProfessional { get; set; }
        
        [RegularExpression(@"^[A-Z]{2}[0-9]{9}[A-Z]{1}[0-9]{2}$", ErrorMessage ="Ongeldige BTW nummer")]
        [DisplayName("Btw-nummer")]
        public string BtwNummer { get; set; }
        [DisplayName("Firma naam")]
        public string FirmaNaam { get; set; }
    }
    
}
