using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels
{
    public class BeoordelingMetKlantNaamViewModel :Beoordeling
    {
        public BeoordelingMetKlantNaamViewModel(string commentaar, ScoreLijst scoreLijst, int klantId, string klantNaam) : base(commentaar, scoreLijst, klantId)
        {
            KlantNaam = klantNaam;
        }

        [DisplayName("Klantnaam")]
        [NotNull]
        public string KlantNaam { get; set; }
        
    }
}
