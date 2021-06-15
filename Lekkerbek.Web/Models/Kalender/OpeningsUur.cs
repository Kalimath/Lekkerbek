using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lekkerbek.Web.Models.Kalender
{
    public class OpeningsUur : ITijdInvulling
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression("(Maandag)|(Dinsdag)|(Woensdag)|(Donderdag)|(Vrijdag)|(Zaterdag)|(Zondag)", ErrorMessage = "Geen dag van de week")]
        public string Dag { get; set; }
        //[RegularExpression("(([0-1][0-9]|[2][0-3]):[0-5][0-9]-([0-1][0-9]|[2][0-4]):[0-5][0-9])|(Gesloten)", ErrorMessage = "Ongeldig tijdstip juiste formaat is xx:xx-xx:xx")]
        [DisplayName("Openingsuren")]
        public string Uur => IsGesloten ? (SluitingsUur.Date + "").Substring(0, 10) + ": Gesloten" : Startuur + " - " + SluitingsUur.TimeOfDay;
        [DisplayName("Sluitingsdag")]
        public bool IsGesloten { get; set; } = false;
        [Required]
        public DateTime Startuur { get; set; }
        [Required]
        public DateTime SluitingsUur { get; set; }

        public List<DateTime> AlleUren()
        {
            List<DateTime> alleUren = new List<DateTime>();
            for(int i= 0; i < SluitingsUur.Hour; i++)
            {
                alleUren.Add(Startuur.AddHours(i));
            }
            return alleUren; 

        }
    }
}
