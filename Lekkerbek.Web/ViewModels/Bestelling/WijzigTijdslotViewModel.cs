using Lekkerbek.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.Bestelling
{
    [NotMapped]
    public class WijzigTijdslotViewModel
    {
        public int Id { get; set; }
        public int NieuwTijdslotId { get; set; }
        public Tijdslot HuidigTijdslot { get; set; }
        public int HuidigTijdslotId { get; set; }
    }
}
