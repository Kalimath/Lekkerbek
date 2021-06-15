using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.ViewModels.OpeningsUur
{
    [NotMapped]
    public class DagInfoViewModel
    {
        public Models.OpeningsUur OpeningsUur { get; set; }
        public List<Tijdslot> Tijdsloten { get; set; }
        [DisplayName("Aantal beschikbare koks")]
        public int AantalKoksBeschikbaar { get; set; }
        [DisplayName("Koks met vakantie")]
        public List<Gebruiker> KoksVakantieOpDag { get; set; }
        [DisplayName("Koks ziek")]
        public List<Gebruiker> KoksZiekOpDag { get; set; }
    }
}
