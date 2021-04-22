using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Models.Dtos
{
    public class GebruikerMetRolDto : Gebruiker
    {
        [NotMapped]
        public string Rol { get; set; }
    }
}
