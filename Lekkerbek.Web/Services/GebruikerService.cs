using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class GebruikerService: IGebruikerService
    {
        private IdentityContext _context;
        public GebruikerService(IdentityContext context)
        {
            _context = context;
        }

        public ICollection<Gebruiker> GetGebruikers()
        {
            try
            {
                return _context.Gebruikers.Include("Bestellingen").Include("Voorkeursgerechten").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Gebruiker>();
            }
        }

        public ICollection<Gebruiker> GetGebruikersMetRolKlant()
        {
            try
            {
                return _context.GebruikersMetRolKlant();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Gebruiker>();
            }
        }

        public Gebruiker GetGebruikerMetRolKlant(int gebruikerId)
        {
            try
            {
                return _context.GebruikersMetRolKlant().Find(gebruiker => gebruiker.Id == gebruikerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen gebruiker vinden met rol KLANT en id: " + gebruikerId);
            }
        }

        public Gebruiker GetGebruiker(int gebruikerId)
        {
            try
            {
                return GetGebruikers().First(gebruiker => gebruiker.Id == gebruikerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen gebruiker vinden met id: " + gebruikerId);
            }
        }

        public async Task<bool> AddGebruiker(Gebruiker nieuweGebruiker)
        {
            bool result = false;
            try
            {
                if (nieuweGebruiker != null && !GebruikerExists(nieuweGebruiker.Id))
                {
                    _context.Gebruikers.Add(nieuweGebruiker);
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    throw new ServiceException("Kon geen gebruiker toevoegen met gebruikersnaam: " + nieuweGebruiker.UserName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                    
            }
            return result;
        }

        public async Task<bool> DeleteGebruiker(int gebruikerId)
        {
            bool result = false;
            try
            {
                if (GebruikerExists(gebruikerId))
                {
                    _context.Gebruikers.Remove(GetGebruiker(gebruikerId));
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    throw new ServiceException("Kon geen gebruiker verwijderen met opgegeven id: " + gebruikerId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public bool GebruikerExists(int gebruikerId)
        {
            return _context.Gebruikers.Any(gebruiker => gebruiker.Id == gebruikerId);
        }

        public string GetHoogsteRolVanGebruiker(int gebruikerId)
        {
            return GebruikerExists(gebruikerId)?_context.GebruikerHoogsteRol(gebruikerId):throw new ServiceException("Kon rol van gebruiker niet opvragen: ID is ongeldig");
        }

    }
}
