using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models
{
    public class Tijdslot : ITijdInvulling
    {
        public int Id { get; set; }
        [Required]
        public DateTime Tijdstip { get; set; }
        public bool IsVrij { get; set; } = true;
        [DisplayName("In bereiding door")]
        public Gebruiker InGebruikDoorKok { get; set; } = null;
        public Tijdslot(DateTime tijdstip)
        {
            Tijdstip = tijdstip; 
        }
        public Tijdslot()
        {
        }

    }
}
