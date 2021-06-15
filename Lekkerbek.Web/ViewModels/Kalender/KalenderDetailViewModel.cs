using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels.Kalender
{
    [NotMapped]
    public class KalenderDetailViewModel
    {
        public int AantalKoksBeschikbaar { get; set; }
        public List<Tijdslot> Tijdsloten { get; set; }
        public Models.Kalender.OpeningsUur Openingsuren { get; set; }
    }
}
