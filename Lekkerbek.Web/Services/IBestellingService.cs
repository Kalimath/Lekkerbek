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

        public ICollection<Bestelling> GetBestellingenVanKlant(int klantId);
        public bool SetBestellingen(ICollection<Bestelling> bestellingen);
        public bool AddBestelling(Bestelling bestelling);
        public bool DeleteBestelling(int id);
    }

}
