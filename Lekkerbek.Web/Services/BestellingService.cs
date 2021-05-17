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
            if (_context.Bestellingen.Any(bestelling => bestelling.Id == id))
            {
                return _context.Bestellingen.Include("Klant").Include("Tijdslot").Include("GerechtenLijst")
                    .FirstAsync(bestelling1 => bestelling1.Id == id).Result;
            }
            else
            {
                throw new ServiceException("Kon geen bestelling vinden met opgegeven id: "+id);
            }
            
        }

        public ICollection<Bestelling> GetBestellingenVanKlant(int klantId)
        {
            throw new NotImplementedException();
        }

        public bool SetBestellingen(ICollection<Bestelling> bestellingen)
        {
            throw new NotImplementedException();
        }

        public bool AddBestelling(Bestelling bestelling)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBestelling(int id)
        {
            throw new NotImplementedException();
        }
    }
}
