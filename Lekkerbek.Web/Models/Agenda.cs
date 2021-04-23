using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models
{
    public  class Agenda
    {
        public int Id { get; set; }
        public ICollection<Tijdslot> Tijdsloten { get; set; }
        public Agenda()
        {
            Tijdsloten = new HashSet<Tijdslot>();
        }
        public ICollection<Tijdslot> GetBeschikbareTijdsloten()
        {
            return Tijdsloten.AsQueryable().Where(tijdslot => tijdslot.IsVrij).ToList(); 
        }

    }
}
