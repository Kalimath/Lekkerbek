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
        private readonly IdentityContext _context;

        public BestellingService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<List<Bestelling>> GetAlleBestellingen()
        {
            return await _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst").ToListAsync();
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
                try
                {
                    return GetAlleBestellingen().Result.Where(bestelling => bestelling.KlantId == klantId).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new ServiceException(e.Message);
                }
            }
            else
            {
                throw new ServiceException("Kon geen bestelling van klant vinden met opgegeven klant-id: "+klantId);
            }
        }

        public async Task SetBestellingen(ICollection<Bestelling> bestellingen)
        {
            try
            {
                _context.Bestellingen.AddRange(bestellingen);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }

            
        }

        public async Task AddBestelling(Bestelling bestelling)
        {
            try
            {
                _context.Bestellingen.Add(bestelling);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteBestelling(int id)
        {
            try
            {
                if (BestellingExists(id))
                {
                    _context.Bestellingen.Remove(GetBestelling(id));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon bestelling met id: " +id+ " niet verwijderen: bestelling niet in database");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task UpdateBestelling(Bestelling bestelling)
        {
            try
            {
                if (BestellingExists(bestelling.Id))
                {
                    _context.Update(bestelling);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon bestelling met id: " + bestelling.Id + " niet aanpassen: bestelling niet in database");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task AddGerechtAanBestelling(int id, Gerecht gerecht)
        {
            try
            {
                if (gerecht == null)
                {
                    throw new ArgumentNullException("Kon geen leeg gerecht toevoegen aan een bestelling met id: " + id);
                }
                if (_context.Gerechten.Any(gerecht1 => gerecht1.Naam.Equals(gerecht.Naam)))
                {
                    throw new ServiceException("Kon geen onbekend gerecht toevoegen aan een bestelling met id: " + id);
                }
                Bestelling bestelling = null;
                if (!BestellingExists(id))
                {
                    throw new ServiceException("Kon geen gerechten toevoegen aan een bestelling met id: "+id+ " die niet in de database zit");
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
                _context.Update(bestelling);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteGerechtVanBestelling(string gerechtNaam, int bestellingId)
        {
            try
            {
                if (GerechtExistsInBestelling(gerechtNaam, bestellingId))
                {
                    GetBestelling(bestellingId).GerechtenLijst.Remove(_context.Gerecht.Find(gerechtNaam));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon gerecht (Naam=" + gerechtNaam + ") niet verwijderen uit bestelling met id: " + bestellingId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

         
        public bool BestellingExists(int bestellingId)
        {
            return _context.Bestellingen.Any(bestelling => bestelling.Id == bestellingId);
        }

        public bool GerechtExistsInBestelling(string gerechtNaam, int bestellingId)
        {
            bool result = false;
            if (gerechtNaam != null && !gerechtNaam.Equals(""))
            {
                if (BestellingExists(bestellingId) && GetBestelling(bestellingId).GerechtenLijst.Any(gerecht => gerecht.Naam.Equals(gerechtNaam)))
                {
                    result = true;
                }
            }
            return result;
        }

        public int AantalBestellingenVanKlant(int klantId)
        {
            return GetBestellingenVanKlant(klantId).ToList().Count;
        }
    }
}
