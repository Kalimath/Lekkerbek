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
                var aantalKoksBeschikbaarOpDatum = Kalender.AantalKoksBeschikbaarOpDatum(item.Startuur.Date);
                // foreach uur in openingsuren
                foreach (var uur in item.AlleUren())
                {
                    if (item.Startuur.Date == uur.Date && item.SluitingsUur.Hour > uur.Hour)
                    {
                        var aantalMinuten = item.Startuur.Hour;
                        // om het kwartier x aantal tijdsloten
                        while (aantalMinuten <= item.Startuur.Hour + 45)
                        {
                            if (new DateTime(uur.Year, uur.Month, uur.Day, uur.Hour, aantalMinuten, 0) >= item.SluitingsUur)
                            {
                                aantalMinuten += 70;
                            }
                            // foreach beschikbare kok
                            for (int i = 0; i < aantalKoksBeschikbaarOpDatum; i++)
                            {
                                Kalender.Tijdsloten.Add(new Tijdslot(new DateTime(uur.Year, uur.Month, uur.Day, uur.Hour, aantalMinuten, 0)));
                            }
                            aantalMinuten += 15;
                        }

                    }
                }
                
            }
        }
        
    }
}
