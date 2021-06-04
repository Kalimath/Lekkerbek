using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        public IEnumerable<string> GerechtenNamen { get; set; }
        public IEnumerable<SelectListItem> Tijdsloten{ get; set; }
        public DateTime Tijdstip{ get; set; }
        public DateTime Levertijd { get; set; }
        public int AantalMaaltijden { get; set; }
        public string Opmerkingen { get; set; }
    }
}
