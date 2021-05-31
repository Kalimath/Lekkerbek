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
                return GetAlleTijdsloten().FirstOrDefault(tijdslot => tijdslot.Id == tijdslotId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen tijdslot vinden: " + tijdslotId);
            }
        }

        public ICollection<Tijdslot> GetAlleTijdsloten()
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

        public ICollection<Tijdslot> GetVrijeTijdsloten()
        {
            throw new NotImplementedException();
        }

        public ICollection<Tijdslot> GetGereserveerdeTijdsloten()
        {
            throw new NotImplementedException();
        }

        public ICollection<Tijdslot> GetTijdslotenVanKok(int kokId)
        {
            return GetTijdslotenVanKok(kokId).ToList();
        }

        public async Task AddTijdslot(Tijdslot nieuwTijdslot)
        {
            try
            {
                if (nieuwTijdslot != null && !TijdslotExists(nieuwTijdslot))
                {
                    await _context.Tijdslot.AddAsync(nieuwTijdslot);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon geen tijdslot toevoegen met tijdstip: " + nieuwTijdslot.Tijdstip.Date);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //TODO
        public async Task UpdateTijdslot(Tijdslot updatedTijdslot)
        {
            try
            {
                if (TijdslotExists(updatedTijdslot.Id))
                {
                    _context.Update(updatedTijdslot);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon tijdslot met id: " + updatedTijdslot.Id + " niet aanpassen: tijdslot niet in database");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteTijdslot(int tijdslotId)
        {
            try
            {
                if (TijdslotExists(tijdslotId))
                {
                    _context.Tijdslot.Remove(GetTijdslot(tijdslotId));
                    await _context.SaveChangesAsync();
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

        }

        public bool TijdslotExists(int tijdslotId)
        {
            return _context.Tijdslot.Any(tijdslot => tijdslot.Id == tijdslotId);
        }

        public bool TijdslotExists(Tijdslot tijdslot)
        {
            return _context.Tijdslot.Any(tijdslot1 => tijdslot1.Id == tijdslot.Id);
        }
    }
}
