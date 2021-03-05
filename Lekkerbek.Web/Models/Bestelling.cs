using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Bestelling : Tijdstipt
    {

        public int Id { get; set; }
        public Klant Klant { get; set; }
        public int AantalMaaltijden { get; set; }

        internal static object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        //Prop voor tijdslot --> voorstel een klasse Tijdstip -- ok

    }
}
