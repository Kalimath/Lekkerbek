using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;


namespace Lekkerbek.Web.Services
{
    interface ITijdslotService
    {
        public Tijdslot GetTijdslot(string Tijdslot);
        public Task AddTijdslot(Tijdslot tijdslot);
        public ICollection<Tijdslot> GetTijdsloten();
        public Task UpdateTijdslot(Tijdslot updatedTijdslot);
        public Task DeleteTijdslot(string Tijdslot);
        public bool TijdslotExists(Tijdslot tijdslot);
    }
}
