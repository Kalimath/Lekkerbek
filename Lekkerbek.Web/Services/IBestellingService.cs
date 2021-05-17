using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Services
{
    public interface IBestellingService
    {
        public Task<List<Bestelling>> GetAlleBestellingen();
        public Bestelling GetBestelling(int id);

        public List<Bestelling> GetBestellingenVanKlant(int klantId);
        public Task<bool> SetBestellingen(ICollection<Bestelling> bestellingen);
        public Task<bool> AddBestelling(Bestelling bestelling);
        public Task<bool> DeleteBestelling(int id);
    }

}
