using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels.Kalender
{
    public class KalenderDetailViewModel
    {
        public int AantalKoksVrij { get; set; }
        public Tijdslot Tijdslot { get; set; }
        public Models.OpeningsUur Openingsuren { get; set; }
    }
}
