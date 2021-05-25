using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lekkerbek.Web.Models
{
    public class ScoreLijst
    {
        public int Id { get; set; }

        [DisplayName("Service")]
        [Required]
        [NotNull]
        public double ServiceScore { get; set; }

        [DisplayName("Eten en drinken")]
        [Required]
        [NotNull]
        public double EtenEnDrinkenScore { get; set; }

        [DisplayName("Prijs-kwaliteit")]
        [Required]
        [NotNull]
        public double PrijsKwaliteitScore { get; set; }

        [DisplayName("Hygiëne")]
        [Required]
        [NotNull]
        public double HygieneScore { get; set; }

        public virtual int BeoordelingsId { get; set; }
    }
}