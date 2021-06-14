using Lekkerbek.Web.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.EmailKlant
{
    [NotMapped]
    public class IndexViewModel
    {
        public int Klant { get; set; }
        public bool Pdf { get; set; }
    }
}
