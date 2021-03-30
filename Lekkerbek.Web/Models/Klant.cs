using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models
{
    public class Klant: Gebruiker
    {
        public Klant()
        {
            Bestellingen = new HashSet<Bestelling>();
            /*Rol = new Role(RollenEnum.Klant.ToString());*/
            
        }
        public int Getrouwheidsscore { get; set; }
        public virtual ICollection<Gerecht> Voorkeursgerechten { get; set; } = new List<Gerecht>();
        public virtual ICollection<Bestelling> Bestellingen { get; set; }
    }
    
}
