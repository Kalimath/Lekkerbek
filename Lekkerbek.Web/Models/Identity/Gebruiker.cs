using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lekkerbek.Web.Models.Identity
{
    public class Gebruiker : IdentityUser<int>
    {
        private DateTime _geboortedatum;
        public string Naam { get; set; }
        public string Adres { get; set; }


        [DataType(DataType.Date)]
        public DateTime Geboortedatum
        {
            get => _geboortedatum.Date;
            set => _geboortedatum = value;
        }
    }
    
}
