using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lekkerbek.Web.Models
{
    public class Gerecht
    {
        private double _prijs;

        [Key]
        public string Naam { get; set; }

        public Gerecht()
        {
            Bestellingen = new HashSet<Bestelling>();
        }
        [Required]
        public string CategorieId { get; set; }
        public virtual Categorie Categorie { get; set; }

        public double Prijs
        {
            get => _prijs;
            set => _prijs = value;
        }

        public virtual ICollection<Bestelling> Bestellingen { get; set; }

        
    }
}
