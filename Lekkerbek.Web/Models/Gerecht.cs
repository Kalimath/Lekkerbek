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
        public string CategorieId { get; set; }
        public virtual Categorie Categorie { get; set; }
        public double Prijs { get; set; }
    }
}
