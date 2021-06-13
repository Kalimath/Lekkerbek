using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lekkerbek.Web.Models
{
    
    public class Categorie : IDisposable
    {
        [Key]
        [Required]
        public string Naam { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
