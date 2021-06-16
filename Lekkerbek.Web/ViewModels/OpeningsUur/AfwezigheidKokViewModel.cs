using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.OpeningsUur
{
    public class AfwezigheidKokViewModel
    {
        public int KokId { get; set; }
        [DisplayName("Is kok ziek?")]
        public bool IsZiek { get; set; }
        public int OpeningsUurId { get; set; }
    }
}
