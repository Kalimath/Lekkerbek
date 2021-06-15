using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;


namespace Lekkerbek.Web.Services
{
    public interface ITijdslotService
    {
        public Tijdslot GetTijdslot(int TijdslotId);
        public Task AddTijdslot(Tijdslot tijdslot);
        public ICollection<Tijdslot> GetAlleTijdsloten();
        public ICollection<Tijdslot> GetVrijeTijdsloten();
        public ICollection<Tijdslot> GetGereserveerdeTijdsloten();
        public ICollection<Tijdslot> GetTijdslotenVanKok(int kokId);
        public Task UpdateTijdslot(Tijdslot updatedTijdslot);
        public Task DeleteTijdslot(int tijdslotId);
        public List<Tijdslot> GetAlleTijdslotenZonderDuplicates(); 
        public bool TijdslotExists(int tijdslotId);
        public bool TijdslotExists(Tijdslot tijdslot);
    }
}
