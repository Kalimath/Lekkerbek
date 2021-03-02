using System;
using System.Linq;

namespace Lekkerbek.Web.Models
{
    public enum CategorieEnum
    {
        voorgerecht,hoofdgerecht,dessert
    }
    public class Gerecht
    {


        public int Id { get; set; }

        public string Omschrijving { get; set; }
        public CategorieEnum Categorie { get; set; }
        public double Prijs { get; set; }
    }
}
