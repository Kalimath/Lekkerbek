using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public sealed class Tijdstippen
    {
        public static List<Tijdslot> Tijdsloten = new List<Tijdslot> {
            new Tijdslot("8:00"), new Tijdslot("8:00"), new Tijdslot("8:15"), new Tijdslot("8:15"),
            new Tijdslot("8:30"), new Tijdslot("8:30"),new Tijdslot("8:45"), new Tijdslot("8:45")
        };
        public static List<Tijdslot> GetBeschikbaar()
        {
            return Tijdsloten.Where(tijdslot => tijdslot.IsVrij).ToList(); 
        }

    }
}
