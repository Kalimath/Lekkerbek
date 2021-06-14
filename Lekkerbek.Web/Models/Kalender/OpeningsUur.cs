using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class OpeningsUur : ITijdInvulling
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression("(Maandag)|(Dinsdag)|(Woensdag)|(Donderdag)|(Vrijdag)|(Zaterdag)|(Zondag)", ErrorMessage = "Geen dag van de week")]
        public string Dag { get; set; }
        //[RegularExpression("(([0-1][0-9]|[2][0-3]):[0-5][0-9]-([0-1][0-9]|[2][0-4]):[0-5][0-9])|(Gesloten)", ErrorMessage = "Ongeldig tijdstip juiste formaat is xx:xx-xx:xx")]
        public string Uur { get {return Startuur.Hour + " - " + SluitingsUur.Hour;  } }
        public DateTime Startuur { get; set; }
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

        public override string ToString()
        {
            return Startuur.Hour + " - " + SluitingsUur.Hour;
        }
    }
}
