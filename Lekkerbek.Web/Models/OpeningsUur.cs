using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class OpeningsUur
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression("(Maandag)|(Dinsdag)|(Woensdag)|(Donderdag)|(Vrijdag)|(Zaterdag)|(Zondag)", ErrorMessage = "Geen dag van de week")]
        public string Dag { get; set; }
        [RegularExpression("(([0-1][0-9]|[2][0-3]):[0-5][0-9]-([0-1][0-9]|[2][0-4]):[0-5][0-9])|(Gesloten)", ErrorMessage = "Ongeldig tijdstip juiste formaat is xx:xx-xx:xx")]
        public string Uur { get; set; }
        public DateTime Datum { get; set; }
    }
}
