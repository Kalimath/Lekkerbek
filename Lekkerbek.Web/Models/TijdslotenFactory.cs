using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Models
{
    public class TijdslotenFactory
    {
        private readonly double _tijdslotDuur;

        public TijdslotenFactory(int tijdslotDuur)
        {
            _tijdslotDuur = tijdslotDuur;
        }

        public List<Tijdslot> VulKalender(List<OpeningsUur> openingsUren, int aantalKoksOpDatum)
        {
            var nieuweTijdsloten = new List<Tijdslot>();
            if (aantalKoksOpDatum !=0 && openingsUren !=null)
            {
                // foreach openingsdag
                foreach (var item in openingsUren)
                {
                    var nieuwTijdslotMoment = item.Startuur;

                    //Zolang het aankomende nieuwTijdslotMoment valt voor het sluitingsuur van een bepaalde dag
                    while (nieuwTijdslotMoment < item.SluitingsUur)
                    {
                        // foreach beschikbare kok
                        for (int i = 0; i < aantalKoksOpDatum; i++)
                        {
                            //nieuw tijdslot toevoegen met nieuwTijdslotMoment
                            nieuweTijdsloten.Add(new Tijdslot(nieuwTijdslotMoment));
                        }
                        //volgende nieuweTijdslotMoment 15 minuten later dan huidig
                        nieuwTijdslotMoment.AddMinutes(_tijdslotDuur);
                    }
                }
            }
            return nieuweTijdsloten;
        }

        public List<Tijdslot> VulKalenderVoorDatum(OpeningsUur openingsUur, int aantalKoksOpDatum)
        {
                var nieuweTijdsloten = new List<Tijdslot>();
                
                if (aantalKoksOpDatum != 0 && openingsUur != null)
                {
                    var nieuwTijdslotMoment = openingsUur.Startuur;
                //Zolang het aankomende nieuwTijdslotMoment valt voor het sluitingsuur van een bepaalde dag
                while (nieuwTijdslotMoment < openingsUur.SluitingsUur)
                    {
                        // foreach beschikbare kok
                        for (int i = 0; i < aantalKoksOpDatum; i++)
                        {
                            //nieuw tijdslot toevoegen met nieuwTijdslotMoment
                            nieuweTijdsloten.Add(new Tijdslot(nieuwTijdslotMoment));
                        }

                        //volgende nieuweTijdslotMoment 15 minuten later dan huidig
                        nieuwTijdslotMoment = nieuwTijdslotMoment.AddMinutes(_tijdslotDuur);
                    }
                }

                return nieuweTijdsloten;
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
