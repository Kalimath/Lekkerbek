using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.Bestelling
{
    public class CreateViewModel
    {
        //public List<Gebruiker> Klanten { get; set; }
        public IEnumerable<SelectListItem> KlantenLijst { get; set; }
        public int KlantId { get; set; }
        public IEnumerable<SelectListItem> GerechtenLijst { get; set; }
        [Required]
        [DisplayName("Aantal maaltijden")]
        public IEnumerable<string> GerechtenNamen { get; set; }
        public IEnumerable<SelectListItem> Tijdsloten{ get; set; }
        public DateTime Tijdstip{ get; set; }
        [Range(1, 999)]
        public int AantalMaaltijden { get; set; }
        public string Opmerkingen { get; set; }
    }
}
