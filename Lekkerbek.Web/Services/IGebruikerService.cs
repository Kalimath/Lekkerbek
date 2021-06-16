using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Services
{
    public interface IGebruikerService
    {
        public ICollection<Gebruiker> GetGebruikers();
        public ICollection<Gebruiker> GetGebruikersMetRolKlant();
        public ICollection<Gebruiker> GetGebruikersMetRolKok();
        public Gebruiker GetGebruikerMetRolKlant(int gebruikerId);
        public Gebruiker GetGebruikerMetRolKok(int gebruikerId);
        public Gebruiker GetGebruiker(int gebruikerId);
        public Gebruiker GetGebruikerInfo(Gebruiker gebruiker);
        public List<string> GetGebruikerRollen();
        public Task<bool> AddGebruiker(Gebruiker nieuweGebruiker, string passwordHash, string rol);
        public Task<bool> UpdateGebruiker(Gebruiker updatedGebruiker);
        public Task<bool> DeleteGebruiker(int gebruikerId);
        public bool GebruikerExists(int gebruikerId);
        public string GetHoogsteRolVanGebruiker(int gebruikerId);
    }
}
