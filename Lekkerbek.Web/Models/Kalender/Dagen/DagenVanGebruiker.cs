using System;
using System.Collections.Generic;

namespace Lekkerbek.Web.Models.Kalender
{
    public abstract class DagenVanGebruiker
    {
        public int Id { get; set; }
        public virtual ICollection<Dag> Dagen { get; set; }
        public virtual int GebruikerId { get; set; }

        protected DagenVanGebruiker(int gebruikerId)
        {
            Dagen = new HashSet<Dag>();
            GebruikerId = gebruikerId;
        }
    }
}