using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public sealed class GerechtenDBTemp
    {
        private static List<Gerecht> Gerechten { get; set; } = new List<Gerecht> {
            new Gerecht{ Id = 1, Omschrijving = "Frieten", Categorie = CategorieEnum.hoofdgerecht, Prijs = 12},
            new Gerecht{ Id = 2, Omschrijving = "Rijstpap", Categorie = CategorieEnum.dessert, Prijs = 300},
            new Gerecht{ Id = 3, Omschrijving = "Scampi", Categorie = CategorieEnum.voorgerecht, Prijs = 15}
        };
        public static Gerecht GetGerecht(int id)
        {
            return Gerechten.FirstOrDefault(c => c.Id == id);
        }
        public static List<Gerecht> getGerechten()
        {
            return Gerechten;
        }
        public static void UpdateGerecht(int id, string omschrijving, CategorieEnum categorie, double prijs)
            
        {
            Gerecht gerechtToUpdate = GetGerecht(id);
            if (gerechtToUpdate != null)
            {
                gerechtToUpdate.Omschrijving = omschrijving;
                gerechtToUpdate.Prijs = prijs;
                gerechtToUpdate.Categorie = categorie;
            }
        }
        public static void AddGerecht(Gerecht gerecht)
        {
            int nextId = Gerechten.Select(c => c.Id).Max() + 1;
            gerecht.Id = nextId;
            Gerechten.Add(gerecht);
        }
    }
}
