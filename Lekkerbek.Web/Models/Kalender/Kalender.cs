using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models.Kalender
{
    [NotMapped]
    public sealed class Kalender
    {
        private static Kalender _instance = null;

        public static TijdslotenFactory TijdslotenFactory;
        public List<OpeningsUur> Openingsuren { get; set; } = new List<OpeningsUur>();
        public List<Tijdslot> Tijdsloten { get; set; } = new List<Tijdslot>();

        public List<ZiekteDagenVanGebruiker> ZiektedagenKoks { get; set; }
        public List<VerlofDagenVanGebruiker> VerlofdagenKoks { get; set; }
        private Kalender()
        {
            TijdslotenFactory = new TijdslotenFactory(15);
            TijdslotenFactory.VulKalender();
        }
        public static Kalender Instance => _instance ??= new Kalender();

        public int AantalKoksBeschikbaarOpDatum(DateTime datum)
        {
            var aantal = 0;
            foreach (var item in VerlofdagenKoks)
            {
                foreach (var item2 in ZiektedagenKoks)
                {
                    if (item.Dagen.All(time => time.Datum.Date != datum.Date) && item2.Dagen.All(time => time.Datum.Date != datum.Date))
                    {
                        aantal++;
                    }
                }
            }
            return 0;
        }
    }
}
