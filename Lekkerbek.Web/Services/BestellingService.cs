using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class BestellingService : IBestellingService
    {
        private IdentityContext _context;

        public BestellingService(IdentityContext context)
        {
            _context = context;
        }
        public Task<List<Bestelling>> GetAlleBestellingen()
        {
            return _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst").ToListAsync();
        }

        public Bestelling GetBestelling(int id)
        {
            if (BestellingExists(id))
            {
                return GetAlleBestellingen().Result.FirstOrDefault(bestelling1 => bestelling1.Id == id);
            }
            else
            {
                throw new ServiceException("Kon geen bestelling vinden met opgegeven id: "+id);
            }
            
        }

        public List<Bestelling> GetBestellingenVanKlant(int klantId)
        {
            if (_context.GebruikersMetRolKlant().Any(gebruiker => gebruiker.Id == klantId))
            {
                return GetAlleBestellingen().Result.Where(bestelling => bestelling.KlantId == klantId).ToList();
            }
            else
            {
                throw new ServiceException("Kon geen bestelling van klant vinden met opgegeven klant-id: "+klantId);
            }
        }

        public async Task<bool> SetBestellingen(ICollection<Bestelling> bestellingen)
        {
            try
            {
                _context.Bestellingen.AddRange(bestellingen);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            
        }

        public async Task<bool> AddBestelling(Bestelling bestelling)
        {
            try
            {
                _context.Bestellingen.Add(bestelling);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public Task<bool> DeleteBestelling(int id)
        {
            if (BestellingExists(id))
            {
                try
                {
                    _context.Bestellingen.Remove(GetBestelling(id));
                    _context.SaveChangesAsync();
                    return Task.FromResult(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Task.FromResult(false);
                }
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public bool BestellingExists(int bestellingId)
        {
            return _context.Bestellingen.Any(bestelling => bestelling.Id == bestellingId);
        }
    }
}
