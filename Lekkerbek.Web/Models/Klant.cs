using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Klant
    {
        

        public int Id { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }
        public DateTime Geboortedatum { get; set; }
        public int Getrouwheidsscore { get; set; }
        public List<Gerecht> Voorkeursgerechten { get; set; } = new List<Gerecht>();

    }
}
