using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class TijdslotenFactory
    {
        private readonly int _tijdslotDuur;

        public TijdslotenFactory(int tijdslotDuur)
        {
            _tijdslotDuur = tijdslotDuur;
        }

        public void VulKalender()
        {
            // foreach openingsdag
            foreach (var item in Kalender.Kalender.Instance.Openingsuren)
            {
                var nieuwTijdslotMoment = item.Startuur;
                var aantalKoksBeschikbaarOpDatum = Kalender.Kalender.Instance.AantalKoksBeschikbaarOpDatum(item.Startuur.Date);

                //Zolang het aankomende nieuwTijdslotMoment valt voor het sluitingsuur van een bepaalde dag
                while (nieuwTijdslotMoment < item.SluitingsUur)
                {
                    // foreach beschikbare kok
                    for (int i = 0; i < aantalKoksBeschikbaarOpDatum; i++)
                    {
                        //nieuw tijdslot toevoegen met nieuwTijdslotMoment
                        Kalender.Kalender.Instance.Tijdsloten.Add(new Tijdslot(nieuwTijdslotMoment));
                    }
                    //volgende nieuweTijdslotMoment 15 minuten later dan huidig
                    nieuwTijdslotMoment.AddMinutes(_tijdslotDuur);
                }
            }
        }

        /*public void VulKalender()
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
                                Kalender.Tijdslot.Add(new Tijdslot(new DateTime(uur.Year, uur.Month, uur.Day, uur.Hour, aantalMinuten, 0)));
                            }
                            aantalMinuten += 15;
                        }

                    }
                }

            }
        }*/

    }
}
