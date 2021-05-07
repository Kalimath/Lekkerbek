using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels
{
    [NotMapped]
    public class GebruikerMetRolViewModel
    {
        public string Gebruikersnaam { get; set; }
        private DateTime _geboortedatum;
        public string Adres { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geboortedatum
        {
            get => _geboortedatum.Date;
            set => _geboortedatum = value;
        }

        /*public int Getrouwheidsscore { get; set; } = 0;*/
        public string Email { get; set; }
        public string Rol { get; set; }
        public int Id { get; set; }
    }
}
