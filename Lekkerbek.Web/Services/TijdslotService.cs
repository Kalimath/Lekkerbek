using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class TijdslotService : ITijdslotService
    {
        private IdentityContext _context;

        public TijdslotService(IdentityContext context)
        {
            _context = context;
        }

        public Tijdslot GetTijdslot(int tijdslotId)
        {
            try
            {
                return GetTijdsloten().FirstOrDefault(tijdslot => tijdslot.Id == tijdslotId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen tijdslot vinden: " + tijdslotId);
            }
        }

        public async Task<bool> AddTijdslot(Tijdslot nieuweTijdslot)
        {
            bool result = false;
            try
            {
                if (nieuweTijdslot != null && !TijdslotExists(nieuweTijdslot))
                {
                    _context.Tijdslot.Add(nieuweTijdslot);
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    throw new ServiceException("Kon geen gebruiker toevoegen met gebruikersnaam: " + nieuweTijdslot.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return result;
        }

        public async Task<bool> DeleteTijdslot(int tijdslotId)
        {
            bool result = false;
            try
            {
                if (TijdslotExists(tijdslotId))
                {
                    _context.Gebruikers.Remove(GetTijdsloten(tijdslotId));
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    throw new ServiceException("Kon geen gebruiker verwijderen met opgegeven id: " + tijdslotId);
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
            return GebruikerExists(gebruikerId) ? _context.GebruikerHoogsteRol(gebruikerId) : throw new ServiceException("Kon rol van gebruiker niet opvragen: ID is ongeldig");
        }

        public Task UpdateTijdslot(Tijdslot updatedTijdslot)
        {
            throw new NotImplementedException();
        }

        public Tijdslot GetTijdslot(string Tijdslot)
        {
            throw new NotImplementedException();
        }

        public Task AddTijdslot(Tijdslot tijdslot)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tijdslot> GetTijdslot()
        {
            throw new NotImplementedException();
        }

        public Task DeleteTijdslot(string Tijdslot)
        {
            throw new NotImplementedException();
        }

        public bool TijdslotExists(Tijdslot tijdslot)
        {
            throw new NotImplementedException();
        }

        Tijdslot ITijdslotService.GetTijdslot(string Tijdslot)
        {
            throw new NotImplementedException();
        }

        public Task AddTijdslot(Tijdslot tijdslot)
        {
            throw new NotImplementedException();
        }

        ICollection<Tijdslot> ITijdslotService.GetTijdslot()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTijdslot(Tijdslot updatedTijdslot)
        {
            throw new NotImplementedException();
        }

        public bool TijdslotExists(Tijdslot tijdslot)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tijdslot> GetTijdsloten()
        {
            try
            {
                return _context.Tijdslot.Include("InGebruikDoorKok").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }
    }
}
