using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Controllers;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class BeoordelingService : IBeoordelingService
    {
        private IdentityContext _context;

        public BeoordelingService(IdentityContext context)
        {
            _context = context;
        }

        public Beoordeling GetBeoordeling(int beoordelingId)
        {
            try
            {
                return _context.Beoordelingen.Include(beoordeling => beoordeling.ScoreLijst)
                    .FirstOrDefault(beoordeling => beoordeling.Id == beoordelingId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
            
        }

        public async Task AddBeoordeling(Beoordeling beoordeling)
        {
            try
            {
                if (!BeoordelingExists(beoordeling))
                {
                    await _context.Beoordelingen.AddAsync(beoordeling);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Beoordeling bestaat al");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task UpdateBeoordeling(Beoordeling updatedBeoordeling)
        {
            try
            {
                _context.Beoordelingen.Update(updatedBeoordeling);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
            
        }

        public async Task DeleteBeoordeling(int beoordelingId)
        {
            try
            {
                using (Beoordeling beoordeling = GetBeoordeling(beoordelingId))
                {
                    _context.Beoordelingen.Remove(beoordeling);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Beoordeling> GetBeoordelingen()
        {
            try
            {
                return _context.Beoordelingen.Include(beoordeling => beoordeling.ScoreLijst).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Beoordeling> GetBeoordelingenVanKlant(int klantId)
        {
            try
            {
                return _context.Beoordelingen.Include(beoordeling => beoordeling.ScoreLijst).Where(beoordeling => beoordeling.KlantId == klantId).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public bool BeoordelingExists(Beoordeling beoordeling)
        {
            bool result = false;
            try
            {
                if (_context.Beoordelingen.Any(beoordeling1 => beoordeling1.Id == beoordeling.Id))
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
    }
}
