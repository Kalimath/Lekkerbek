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
        public Task SetBestellingen(ICollection<Bestelling> bestellingen);
        public Task AddBestelling(Bestelling bestelling);
        public Task DeleteBestelling(int id);
        public Task UpdateBestelling(Bestelling bestelling);
        public Task AddGerechtAanBestelling(int id, Gerecht gerecht);
        public Task DeleteGerechtVanBestelling(string gerechtNaam, int bestellingId);
        public bool BestellingExists(int bestellingId);
        public bool GerechtExistsInBestelling(string gerechtNaam, int bestellingId);
        public int AantalBestellingenVanKlant(int klantId);

    }

}
