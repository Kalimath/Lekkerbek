using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Voorkeursgerechten
    {
        
        public int Id { get; set; }

        public string GerechtId { get; set; }

        public virtual Gerecht Gerecht{ get; set; }

        public int KlantId { get; set; }

        public virtual Klant Klant { get; set; }

    }
}
