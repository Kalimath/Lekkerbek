using Lekkerbek.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.ViewModels.Bestelling
{
    [NotMapped]
    public class VoegGerechtenToeViewModel
    {
    
        public int Id { get; set; }
        public IEnumerable<SelectListItem> GerechtenLijst { get; set; }

        public string ToeTeVoegenGerecht { get; set; }
    
    }



    



}
