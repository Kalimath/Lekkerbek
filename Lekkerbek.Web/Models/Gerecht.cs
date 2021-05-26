using Lekkerbek.Web.Models.Identity;
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
        [Required]
        public string Naam { get; set; }

        public Gerecht()
        {
            Bestellingen = new HashSet<Bestelling>();
            VoorkeursgerechtenVanKlanten = new HashSet<Gebruiker>(); 
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
        public virtual ICollection<Gebruiker> VoorkeursgerechtenVanKlanten { get; set; }


        public double PrijsInclBtw()
        {
            var prijsIncl = 0.0;
            if (Categorie.Naam.ToLower().Trim().Equals("alcoholische drank"))
            {
                prijsIncl = Prijs * 1.21;
            }
            else
            {
                prijsIncl = Prijs * 1.06;
            }

            return Math.Round(prijsIncl, 2);
        }

        /*public double Prijs
        {
            get => _prijs;
            set => _prijs = value;
        }
        public double PrijsInclBtw
        {
            get => _prijsInclBtw;
            set
            {
                if (Categorie.Naam.ToLower().Trim().Equals("alcoholische drank"))
                {
                    _prijsInclBtw = value * 1.21;
                }
                else
                {
                    _prijsInclBtw = value * 1.06;
                }
                Math.Round(_prijsInclBtw, 2);
            }
        }*/
    }
}
