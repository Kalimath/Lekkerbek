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
            if (_context.Gebruikers.Any(gebruiker => gebruiker.Id == klantId))
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

        public async Task<bool> DeleteBestelling(int id)
        {
            if (BestellingExists(id))
            {
                try
                {
                    _context.Bestellingen.Remove(GetBestelling(id));
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddGerechtAanBestelling(int id, Gerecht gerecht)
        {
            bool result = false;
            if (gerecht == null)
            {
                throw new ServiceException("Kon geen lege gerechtenlijst toevoegen aan een bestelling met id: " + id);
            }
            try
            {
                Bestelling bestelling = null;
                if (!BestellingExists(id))
                {
                    throw new ServiceException("Kon geen gerechten toevoegen aan een onbestaande bestelling met id: "+id);
                }
                else
                {
                    bestelling = GetBestelling(id);
                }
                    
                if (bestelling.GerechtenLijst == null)
                {
                    bestelling.GerechtenLijst = new List<Gerecht>();
                }
                bestelling.GerechtenLijst.Add(gerecht);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;


        }

        public async Task<bool> deleteGerechtVanBestelling(int gerechtId, int bestellingId)
        {
            bool result = false;
            try
            {
                if (BestellingExists(bestellingId))
                {
                    //todo
                }
                else
                {
                    throw new ServiceException("Kon geen gerecht met id: " + gerechtId+
                                               " verwijderen uit een onbestaande bestelling met id: " + bestellingId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        public bool BestellingExists(int bestellingId)
        {
            return _context.Bestellingen.Any(bestelling => bestelling.Id == bestellingId);
        }
    }
}
