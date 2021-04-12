using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Tijdslot
    {
        public int Id { get; set; }
        [Required]
        public DateTime Tijdstip { get; set; }
        public bool IsVrij { get; set; } = true;

        public Tijdslot(DateTime tijdstip)
        {
            Tijdstip = tijdstip; 
        }
        public Tijdslot()
        {
            
        }
        public virtual Kok Kok { get; set; }

    }
}
