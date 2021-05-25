using Lekkerbek.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Services
{
    public interface IBeoordelingService
    {
        public Beoordeling GetBeoordeling(int beoordelingId);
        public Task AddBeoordeling(Beoordeling beoordeling);
        public ICollection<Beoordeling> GetBeoordelingen();
        public ICollection<Beoordeling> GetBeoordelingenVanKlant(int klantId);
        public Task UpdateBeoordeling(Beoordeling updatedBeoordeling);
        public Task DeleteBeoordeling(int beoordelingId);
        public bool BeoordelingExists(Beoordeling beoordeling);
    }
}