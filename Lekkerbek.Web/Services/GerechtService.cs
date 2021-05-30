using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class GerechtService : IGerechtService
    {
        private IdentityContext _context;

        public GerechtService(IdentityContext context)
        {
            _context = context;
        }

        public Gerecht GetGerecht(string gerechtNaam)
        {
            try
            {
                return _context.Gerechten.Include(gerecht => gerecht.Categorie)
                    .FirstOrDefault(gerecht => gerecht.Naam.Trim().Equals(gerechtNaam.Trim()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task AddGerecht(Gerecht gerecht)
        {
            try
            {
                if (!GerechtExists(gerecht))
                {
                    await _context.Gerechten.AddAsync(gerecht);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Gerecht bestaat al");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Gerecht> GetGerechten()
        {
            try
            {
                return _context.Gerechten.Include(gerecht => gerecht.Categorie).OrderBy(gerecht => gerecht.Categorie).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task UpdateGerecht(Gerecht updatedGerecht)
        {
            try
            {
                _context.Gerechten.Update(updatedGerecht);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteGerecht(string gerechtNaam)
        {
            try
            {
                using Gerecht gerecht = GetGerecht(gerechtNaam);
                _context.Gerechten.Remove(gerecht);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public bool GerechtExists(Gerecht gerecht)
        {
            bool result = false;
            try
            {
                if (_context.Gerechten.Any(gerecht1 => gerecht1.Naam.Trim().Equals(gerecht.Naam)))
                {
                    result = true;
                }
            }
            catch (Exception ignore)
            {
                // ignored
            }

            return result;
        }

        public double GerechtenTotaalPrijsAsync(Bestelling bestelling, bool isInclBtw)
        {
            
                var gerechten = _context.Gerechten.Include(gerecht => gerecht.Bestellingen)
                    .Where(gerecht => gerecht.Bestellingen.Contains(bestelling)).ToList();
                double totaalPrijs = 0;
                if (isInclBtw)
                {
                    foreach (var g in gerechten) totaalPrijs += g.PrijsInclBtw();
                }
                else
                {
                    foreach (var g in gerechten) totaalPrijs += g.Prijs;
                }

                if ((_context.Bestellingen.Count(bestelling1 => bestelling1.KlantId == bestelling.KlantId) + 1) >= 3)
                {
                    totaalPrijs *= 0.9;
                }

                return Math.Round(totaalPrijs, 2); ;
            
        }
    }
}
