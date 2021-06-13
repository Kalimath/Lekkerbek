using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Services
{
    public interface IKlachtenService
    {
        public Klacht GetKlacht(int klachtId);
        public Task AddKlacht(Klacht klacht);
        public ICollection<Klacht> GetKlachten();
        public ICollection<Klacht> GetKlachtenVanKlant(int klantId);
        public Task UpdateKlacht(Klacht updatedKlacht);
        public Task DeleteKlacht(int klachtId);
        public bool KlachtExists(Klacht klacht);
        public Task RondKlachtAf(int klachtId);
    }
}
