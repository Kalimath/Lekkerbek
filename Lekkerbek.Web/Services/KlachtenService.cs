using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class KlachtenService : IKlachtenService
    {
        private readonly IdentityContext _context;

        public KlachtenService(IdentityContext context)
        {
            _context = context;
        }

        public Klacht GetKlacht(int klachtId)
        {
            try
            {
                return _context.Klachten.Include(klacht => klacht.Klant)
                    .FirstOrDefault(klacht => klacht.Id == klachtId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task AddKlacht(Klacht klacht)
        {
            try
            {
                if (klacht == null)
                {
                    throw new ServiceException("Klacht is leeg");
                }
                if (!KlachtExists(klacht))
                {
                    await _context.Klachten.AddAsync(klacht);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Klacht bestaat al");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Klacht> GetKlachten()
        {
            try
            {
                return _context.Klachten.Include(klacht => klacht.Klant).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Klacht> GetKlachtenVanKlant(int klantId)
        {
            try
            {
                return GetKlachten().Where(klacht => klacht.Klant.Id == klantId).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task UpdateKlacht(Klacht updatedKlacht)
        {
            try
            {
                if(KlachtExists(updatedKlacht))
                {
                    _context.Klachten.Update(updatedKlacht);
                    await _context.SaveChangesAsync();
                }
                else
                {
                   throw new ServiceException("Klacht bestaat niet");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteKlacht(int klachtId)
        {
            try
            {
                using Klacht klacht = GetKlacht(klachtId);
                _context.Klachten.Remove(klacht);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public bool KlachtExists(Klacht klacht)
        {
            bool result = false;
            try
            {
                if (_context.Klachten.Any(klacht1 => klacht1.Id == klacht.Id))
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

        public async Task RondKlachtAf(int klachtId)
        {
            try
            {
                var klacht = GetKlacht(klachtId);
                klacht.IsAfgehandeld = true;
                _context.Klachten.Update(klacht);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
