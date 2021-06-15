using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.Kalender
{
    [NotMapped]
    public class KalenderIndexViewModel
    {
        public List<DateTime> Momenten { get; set; }
        
    }
}
