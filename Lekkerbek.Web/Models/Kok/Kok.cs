using System;
using System.Collections.Generic;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models.Kok
{
    public class Kok : Gebruiker, IAfwezigheden
    {
        public ICollection<DateTime> Verlofdagen { get; set; }= new List<DateTime>();
        public ICollection<DateTime> Ziektedagen { get; set; } = new List<DateTime>();
        public bool WerktOpDatum(DateTime datum)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
