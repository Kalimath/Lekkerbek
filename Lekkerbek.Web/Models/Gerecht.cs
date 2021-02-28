namespace Lekkerbek.Web.Models
{
    public enum Categorie
    {
        voorgerecht,hoofdgerecht,dessert
    }
    public class Gerecht
    {
        public string Naam { get; set; }
        public Categorie Categorie { get; set; }
        public double Prijs { get; set; }
    }
}
