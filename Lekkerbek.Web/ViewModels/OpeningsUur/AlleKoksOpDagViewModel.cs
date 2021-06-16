using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.OpeningsUur
{
    [NotMapped]
    public class AlleKoksOpDagViewModel
    {
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Rol { get; set; }
        public DateTime Time { get; set; }
    }
}
