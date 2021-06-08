using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models
{
    public class Klacht : IDisposable
    {
        [DisplayName("Klachtnummer")]
        public int Id { get; set; }

        [DisplayName("Tijdstip van indiening")]
        public DateTime Tijdstip { get; set; } = DateTime.Now;

        [Required]
        [NotNull]
        public string Onderwerp { get; set; }

        [Required]
        [NotNull]
        public string Omschrijving { get; set; }

        public bool IsAfgehandeld { get; set; } = false;
        public virtual Gebruiker Klant { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
