using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.Bestelling
{
    public class EditViewModel
    {
        public Gebruiker HuidigeKlant { get; set; }
        public IEnumerable<SelectListItem> Klanten { get; set; }
        public int KlantId { get; set; }
        public List<Gerecht> AlleGerechtNamen { get; set; }
        public IEnumerable<SelectListItem> Tijdslot{ get; set; }
        public Lekkerbek.Web.Models.Bestelling bestelling { get; set; }
    }
}
