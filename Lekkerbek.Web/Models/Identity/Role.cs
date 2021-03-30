using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Lekkerbek.Web.Models.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {

        }
        public Role(string roleName) : base(roleName)
        {
        }
    }

    public enum RollenEnum
    {
        Admin,Kok,Klant,Kassamedewerker,Restaurantuitbater
    }
}
