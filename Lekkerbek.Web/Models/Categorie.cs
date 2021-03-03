using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lekkerbek.Web.Models
{
    
    public class Categorie
    {
        [Key]
        public string Naam { get; set; }
    }
}
