using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Context
{
    public class IdentityContext : IdentityDbContext<Gebruiker,Rol,int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            
        }

        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Rol> Rollen { get; set; }
    }
}
