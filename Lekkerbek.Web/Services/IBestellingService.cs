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
        public void SetBestellingen(ICollection<Bestelling> bestellingen);
        public void AddBestelling(Bestelling bestelling);
        public void DeleteBestelling(int id);
        public void UpdateBestelling(Bestelling bestelling);
        public void AddGerechtAanBestelling(int id, Gerecht gerecht);
        public void DeleteGerechtVanBestelling(string gerechtNaam, int bestellingId);
        public bool BestellingExists(int bestellingId);
        public bool GerechtExistsInBestelling(string gerechtNaam, int bestellingId);
        public Task<int> AantalBestellingenVanKlant(int klantId);

    }

}
