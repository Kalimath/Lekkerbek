using Lekkerbek.Web.Models.Identity;
using Lekkerbek.Web.Models.Kalender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.OpeningsUur
{
    public class RegistreerViewModel 
    {
        public int Id { get; set; }
        public virtual ICollection<Dag> Dagen { get; set; }
        public bool IsVrij { get; set; } = false;

    }
}
