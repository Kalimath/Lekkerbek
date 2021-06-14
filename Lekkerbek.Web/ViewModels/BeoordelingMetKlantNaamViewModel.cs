using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels
{
    [NotMapped]
    public class BeoordelingMetKlantNaamViewModel :Beoordeling
    { 
        public BeoordelingMetKlantNaamViewModel(int id, string titel, string commentaar, ScoreLijst scoreLijst, int klantId, string klantNaam) : base(id, titel, commentaar, scoreLijst, klantId)
        {
            KlantNaam = klantNaam;
        }

        [DisplayName("Klantnaam")]
        [NotNull]
        public string KlantNaam { get; set; }
        
    }
}
