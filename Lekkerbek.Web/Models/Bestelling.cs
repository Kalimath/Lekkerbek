using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Models
{
    public class Bestelling
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        public DateTime Levertijd { get; set; } = new DateTime();
        public string Opmerkingen { get; set; }= "";
        [Required]
        public ICollection<Gerecht> GerechtenLijst { get; set; }
        [Required]
        [Range(1, 999)]
        public int AantalMaaltijden { get; set; }
        [Required]
        public int KlantId { get; set; }
        public virtual Gebruiker Klant { get; set; }
        [Required]
        public Tijdslot Tijdslot { get; set; }
        public bool IsAfgerond { get; set; } = false;
        public bool IsAfhaling { get; set; } = false;
    }
}
