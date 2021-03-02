using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public sealed class KlantenDBTemp
    {
        private static List<Klant> Klanten { get; set; } = new List<Klant> {
            new Klant{ Id = 1, Naam = "Frieda", Adres = "Mechelen", Geboortedatum = DateTime.Now, Getrouwheidsscore = 0},
            new Klant{ Id = 2, Naam = "Rijnaard", Adres = "Leuven", Geboortedatum = DateTime.Now, Getrouwheidsscore = 34},
            new Klant{ Id = 3, Naam = "Jos", Adres = "Haasrode",  Geboortedatum = DateTime.Now, Getrouwheidsscore = 123}
        };
        public static Klant GetKlant(int id)
        {
            return Klanten.FirstOrDefault(c => c.Id == id);
        }
        public static List<Klant> getKlanten()
        {
            return Klanten;
        }
        public static void UpdateKlant(int id, string naam, string adres, DateTime Geboortedatum, int getrouwheidsscore)
            
        {
            Klant klantToUpdate = GetKlant(id);
            if (klantToUpdate != null)
            {
                klantToUpdate.Naam = naam;
                klantToUpdate.Adres = adres;
                klantToUpdate.Geboortedatum = Geboortedatum;
                klantToUpdate.Getrouwheidsscore = getrouwheidsscore;
            }
        }
        public static void AddKlant(Klant klant)
        {
            int nextId = Klanten.Select(c => c.Id).Max() + 1;
            klant.Id = nextId;
            Klanten.Add(klant);
        }
    }
}
