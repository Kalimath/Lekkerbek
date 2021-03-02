using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public sealed class BestellingenDbTemp
    {

        private static List<Bestelling> Bestellingen = new List<Bestelling>
        {
            new Bestelling{Id = 0, Klant = KlantenDBTemp.GetKlant(1), AantalMaaltijden = 4},
            new Bestelling{Id = 1, Klant = KlantenDBTemp.GetKlant(2), AantalMaaltijden = 4}
        }; 

        public static Bestelling GetBestelling(int id)
        {
            return Bestellingen.FirstOrDefault(c => c.Id == id);
        }
        public static List<Bestelling> GetBestellingen()
        {
            return Bestellingen; 
        }
        public static void AddBestelling(Bestelling bestelling)
        {
            int nextId = Bestellingen.Select(c => c.Id).Max() + 1;
            bestelling.Id = nextId;
            Bestellingen.Add(bestelling);
        }
    }
}
