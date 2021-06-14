using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Kalender;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public interface IKalenderService
    {
        public List<OpeningsUur> GetOpeningsUren();
        public Task AddOpeningsUur(OpeningsUur openingsUren);
        public Task UpdateOpeningsUren(OpeningsUur openingsUren);
        public Task AddVerlofDagenVanGebruiker(VerlofDagenVanGebruiker verlofDagen);
        public Task AddZiekteDagenVanGebruiker(ZiekteDagenVanGebruiker ziekteDagen);
        public Task UpdateVerlofDagenVanGebruiker(VerlofDagenVanGebruiker verlofDagen);
        public Task UpdateZiekteDagenVanGebruiker(ZiekteDagenVanGebruiker ziekteDagen);
        public VerlofDagenVanGebruiker GetVerlofDagenVanGebruiker(int gebruikerId);
        public ZiekteDagenVanGebruiker GetZiekteDagenVanGebruiker(int gebruikerId);
        public bool ZiekteDagenGebruikerExists(ZiekteDagenVanGebruiker ziekteDagen);
        public bool VerlofDagenGebruikerExists(VerlofDagenVanGebruiker verlofDagen);
        public List<Dag> GetVerlofDagenVanGebruikers();
        public List<Dag> GetZiekteDagenVanGebruikers();
        public List<Tijdslot> GetTijdslotenOpDag(Dag dag);
        public Tijdslot GetTijdslot(int tijdslotId);
        public Task AddTijdslot(Tijdslot tijdslot);
        public ICollection<Tijdslot> GetAlleTijdsloten();
        public ICollection<Tijdslot> GetVrijeTijdsloten();
        public ICollection<Tijdslot> GetGereserveerdeTijdsloten();
        public ICollection<Tijdslot> GetTijdslotenVanKok(int kokId);
        public Task UpdateTijdslot(Tijdslot updatedTijdslot);
        public Task DeleteTijdslot(int tijdslotId);
        public bool TijdslotExists(int tijdslotId);
        public bool TijdslotExists(Tijdslot tijdslot);
        public int AantalKoksBeschikbaarOpDatum(DateTime datum);
        
    }
}
