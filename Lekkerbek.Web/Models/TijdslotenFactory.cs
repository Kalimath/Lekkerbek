using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class TijdslotenFactory
    {
        private readonly int _tijdslotDuur;
        public Kalender.Kalender Kalender { get; set; }

        public TijdslotenFactory(Kalender.Kalender kalender, int tijdslotDuur)
        {
            _tijdslotDuur = tijdslotDuur;
            Kalender = kalender;
        }

        public void VulKalender()
        {
            var maxAantalKoks = Kalender.MaxAantalKoks;
            // foreach openingsuren
            foreach (var item in Kalender.Openingsuren)
            {
                var aantalKoksBeschikbaarOpDatum = Kalender.AantalKoksBeschikbaarOpDatum(item.Datum);
                Kalender.Tijdsloten.Add();
            }
        }
        
    }
}
