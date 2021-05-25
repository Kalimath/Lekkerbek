using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels
{
    public class BeoordelingMetScoresViewModel : Beoordeling
    {
        [DisplayName("Service")]
        public double ServiceScore { get; set; }

        [DisplayName("Eten en drinken")]
        public double EtenEnDrinkenScore { get; set; }

        [DisplayName("Prijs-kwaliteit")]
        public double PrijsKwaliteitScore { get; set; }

        [DisplayName("Hygiëne")]
        public double HygieneScore { get; set; }


        public BeoordelingMetScoresViewModel(string commentaar, ScoreLijst scoreLijst, int klantId) : base(commentaar, scoreLijst, klantId)
        {
        }

        public BeoordelingMetScoresViewModel(string commentaar, double hygieneScore, double serviceScore, double etenEnDrinkenScore, double prijsKwaliteitScore, int klantId) : base(commentaar, hygieneScore, serviceScore, etenEnDrinkenScore, prijsKwaliteitScore, klantId)
        {
        }
    }
}
