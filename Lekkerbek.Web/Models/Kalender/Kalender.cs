using System;
using System.Collections.Generic;
using System.Linq;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models.Kalender
{
    public class Kalender
    {
        public int MaxAantalKoks { get; set; }
        public List<OpeningsUur> Openingsuren { get; set; } = new List<OpeningsUur>();
        public Dictionary<DateTime, Tijdslot> Tijdsloten { get; set; } = new Dictionary<DateTime, Tijdslot>();
        public Dictionary<Gebruiker, List<DateTime>> ZiektedagenKoks { get; set; } = new Dictionary<Gebruiker, List<DateTime>>();
        public Dictionary<Gebruiker, List<DateTime>> VerlofdagenKoks { get; set; } = new Dictionary<Gebruiker, List<DateTime>>();

        public int AantalKoksBeschikbaarOpDatum(DateTime datum)
        {
            var aantal = 0;
            foreach (var keyValuePair in this.VerlofdagenKoks)
            {
                foreach (var keyValuePair2 in this.ZiektedagenKoks)
                {
                    if (keyValuePair.Value.All(time => time.Date != datum.Date) && keyValuePair2.Value.All(time => time.Date != datum.Date))
                    {
                        aantal++;
                    }
                }
            }
            return 0;
        }
    }
}
