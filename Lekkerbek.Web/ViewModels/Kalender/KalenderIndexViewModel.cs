using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.ViewModels.Kalender
{
    public class KalenderIndexViewModel
    {
        public bool IsOpen { get; set; }
        //public Tijdslot AlleTijdsloten { get; set; }
        public DateTime UurCounter { get; set; }
    }
}
