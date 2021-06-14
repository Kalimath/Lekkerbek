using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Kalender;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class KalenderService : IKalenderService
    {
        private readonly IdentityContext _context;

        public KalenderService(IdentityContext context)
        {
            _context = context;
        }

        public List<OpeningsUur> GetOpeningsUren()
        {
            throw new NotImplementedException();
        }
        // openingsuren: J
        // ziekteDagen: M
        // verlofdagen: R
        public async Task AddOpeningsUren(List<OpeningsUur> openingsUren)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOpeningsUren(List<OpeningsUur> openingsUren)
        {
            throw new NotImplementedException();
        }

        public bool VerlofDagenExists(OpeningsUur openingsUur)
        {
            throw new NotImplementedException();
        }

        public async Task AddVerlofDagenVanGebruiker(VerlofDagenVanGebruiker verlofDagen)
        {
           
        }

        public async Task AddZiekteDagenVanGebruiker(ZiekteDagenVanGebruiker ziekteDagen)
        {
            try
            {
                if (ZiekteDagenGebruikerExists(ziekteDagen))
                {
                    await _context.ZiekteDagenVanGebruikers.AddAsync(ziekteDagen);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon geen ziektedagen toevoegen van gebruiker met id" + ziekteDagen.GebruikerId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateVerlofDagenVanGebruiker(VerlofDagenVanGebruiker verlofDagen)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateZiekteDagenVanGebruiker(ZiekteDagenVanGebruiker ziekteDagen)
        {
            try
            {
                if (ZiekteDagenGebruikerExists(ziekteDagen))
                {
                    _context.Update(ziekteDagen);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon ziektedagen met id: " + ziekteDagen.Id + " niet aanpassen: tijdslot niet in database");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public VerlofDagenVanGebruiker GetVerlofDagenVanGebruiker(int gebruikerId)
        {
            throw new NotImplementedException();
        }

        public ZiekteDagenVanGebruiker GetZiekteDagenVanGebruiker(int gebruikerId)
        {
            try
            {
                return _context.ZiekteDagenVanGebruikers.Include(gebruiker => gebruiker.Dagen).First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public bool ZiekteDagenGebruikerExists(ZiekteDagenVanGebruiker ziekteDagen)
        {
            return _context.ZiekteDagenVanGebruikers.Any(gebruiker => gebruiker.Id == ziekteDagen.Id);
        }

        public bool VerlofDagenGebruikerExists(VerlofDagenVanGebruiker verlofDagen)
        {
            throw new NotImplementedException();
        }

        public List<Dag> GetVerlofDagenVanGebruikers()
        {
            throw new NotImplementedException();
        }

        public List<Dag> GetZiekteDagenVanGebruikers()
        {
            var dagen = new List<Dag>();
            var ziekteDagenVanGebruiker = _context.ZiekteDagenVanGebruikers.Include(gebruiker => gebruiker.Dagen);
            foreach (var ziekteDagen in ziekteDagenVanGebruiker)
            {
                dagen.AddRange(ziekteDagen.Dagen);
            }
            return dagen;
        }

        public List<Tijdslot> GetTijdslotenOpDag(Dag dag)
        {
            return _context.Tijdslot.Include("InGebruikDoorKok")
                .Where(tijdslot => tijdslot.Tijdstip.Date == dag.Datum.Date).ToList();
        }

        public Tijdslot GetTijdslot(int tijdslotId)
        {
            try
            {
                return GetAlleTijdsloten().FirstOrDefault(tijdslot => tijdslot.Id == tijdslotId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen tijdslot vinden: " + tijdslotId);
            }
        }

        public ICollection<Tijdslot> GetAlleTijdsloten()
        {
            try
            {
                return _context.Tijdslot.Include("InGebruikDoorKok").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Tijdslot> GetVrijeTijdsloten()
        {
            return GetAlleTijdsloten().Where(tijdslot => tijdslot.IsVrij).ToList();
        }

        public ICollection<Tijdslot> GetGereserveerdeTijdsloten()
        {
            return GetAlleTijdsloten().Where(tijdslot => !tijdslot.IsVrij).ToList();
        }

        public ICollection<Tijdslot> GetTijdslotenVanKok(int kokId)
        {
            return GetTijdslotenVanKok(kokId).ToList();
        }

        public async Task AddTijdslot(Tijdslot nieuwTijdslot)
        {
            try
            {
                if (nieuwTijdslot != null && !TijdslotExists(nieuwTijdslot))
                {
                    await _context.Tijdslot.AddAsync(nieuwTijdslot);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon geen tijdslot toevoegen met tijdstip: " + nieuwTijdslot.Tijdstip.Date);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateTijdslot(Tijdslot updatedTijdslot)
        {
            try
            {
                if (TijdslotExists(updatedTijdslot.Id))
                {
                    _context.Update(updatedTijdslot);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon tijdslot met id: " + updatedTijdslot.Id + " niet aanpassen: tijdslot niet in database");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteTijdslot(int tijdslotId)
        {
            try
            {
                if (TijdslotExists(tijdslotId))
                {
                    _context.Tijdslot.Remove(GetTijdslot(tijdslotId));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon geen tijdslot verwijderen met opgegeven id: " + tijdslotId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public bool TijdslotExists(int tijdslotId)
        {
            return _context.Tijdslot.Any(tijdslot => tijdslot.Id == tijdslotId);
        }

        public bool TijdslotExists(Tijdslot tijdslot)
        {
            return _context.Tijdslot.Any(tijdslot1 => tijdslot1.Id == tijdslot.Id);
        }

        public int AantalKoksBeschikbaarOpDatum(DateTime datum)
        {
            var aantal = 0;
            foreach (var item in GetVerlofDagenVanGebruikers())
            {
                foreach (var item2 in GetZiekteDagenVanGebruikers())
                {
                    if ((item2.Datum.Date != datum.Date) && (item.Datum.Date != datum.Date))
                    {
                        aantal++;
                    }
                }
            }
            return aantal;
        }
    }
}
