using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Lekkerbek.Web.Models.Kalender;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class KalenderService : IKalenderService
    {
        private readonly IdentityContext _context;
        private readonly TijdslotenFactory _tijdslotenFactory = new TijdslotenFactory(15);

        public KalenderService(IdentityContext context)
        {
            _context = context;
        }

        public List<OpeningsUur> GetOpeningsUren()
        {
            return _context.OpeningsUren.OrderBy(uur =>uur.Startuur).ToList(); 
        }

        public async Task AddOpeningsUur(OpeningsUur openingsUur)
        {
            try
            {
                if (!TijdslotenExistOpDatum(openingsUur.Startuur)&&!openingsUur.IsGesloten)
                {
                    var nieuweTijdsloten = _tijdslotenFactory.VulKalenderVoorDatum(openingsUur, AantalKoksBeschikbaarOpDatum(openingsUur.Startuur));
                    foreach (var item in nieuweTijdsloten)
                    {
                        await _context.AddAsync(item);
                    }
                    
                }
                await _context.OpeningsUren.AddAsync(openingsUur);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                throw new ServiceException(e.Message); 
            }
        }

        public async Task UpdateOpeningsUren(OpeningsUur openingsUur)
        {
            try
            {
                if (OpeningsUurExists(openingsUur.Id))
                {
                    if (!TijdslotenExistOpDatum(openingsUur.Startuur) && !openingsUur.IsGesloten)
                    {
                        var nieuweTijdsloten = _tijdslotenFactory.VulKalenderVoorDatum(openingsUur, AantalKoksBeschikbaarOpDatum(openingsUur.Startuur));
                        await _context.AddAsync(nieuweTijdsloten);
                    }
                    _context.Update(openingsUur);
                    await _context.SaveChangesAsync(); 
                }
                else
                {
                    throw new ServiceException("Kon openingsuur met id: " + openingsUur.Id + "niet aanpassen: openingsuur zit niet in de database");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message); 
            }
        }
        public bool OpeningsUurExists(int OpeningsUurId)
        {
            return _context.OpeningsUren.Any(openingsuur => openingsuur.Id == OpeningsUurId); 
        }
        public async Task AddVerlofDagenVanGebruiker(VerlofDagenVanGebruiker verlofDagen)
        {
            try
            {
                if (verlofDagen != null && !VerlofDagenVanGebruikerExists(verlofDagen))
                {
                    await _context.VerlofDagenVanGebruikers.AddAsync(verlofDagen);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon geen Verlofdag toevoegen met Datum: " + verlofDagen);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

        public async Task UpdateVerlofDagenVanGebruiker(VerlofDagenVanGebruiker UpdatedverlofDagen)
        {
            try
            {
                if (VerlofDagenVanGebruikerExists(UpdatedverlofDagen.Id))
                {
                    _context.Update(UpdatedverlofDagen);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Kon Verlofdag met id: " + UpdatedverlofDagen.Id + " niet aanpassen: Verlofdag niet in database");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
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
            try
            {
                return _context.VerlofDagenVanGebruikers.Include(gebruiker => gebruiker.Dagen).FirstOrDefault(gebruiker => gebruiker.Id == gebruikerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException("Kon geen verlofdagen vinden: " + gebruikerId);
            }
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
            return _context.VerlofDagenVanGebruikers.Any(gebruiker => gebruiker.Id == verlofDagen.Id);
        }

        public List<Dag> GetVerlofDagenVanGebruikers()
        {
            var dagen = new List<Dag>();
            var verlofDagenVanGebruiker = _context.VerlofDagenVanGebruikers.Include(gebruiker => gebruiker.Dagen);
            foreach (var verlofDagen in verlofDagenVanGebruiker)
            {
                dagen.AddRange(verlofDagen.Dagen);
            }
            return dagen;
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
            var aantal = _context.GebruikersMetRolKok().Count;
            foreach (var item2 in GetZiekteDagenVanGebruikers())
            {
                if (datum.Date == item2.Datum.Date)
                {
                    aantal--;
                }
            }
            foreach (var item in GetVerlofDagenVanGebruikers())
            {
                if (datum.Date == item.Datum.Date)
                {
                    aantal--;
                }
            }
            return aantal;
        }

        public bool VerlofDagenVanGebruikerExists(int gebruikerId)
        {
            return _context.VerlofDagenVanGebruikers.Any(Gebruiker => Gebruiker.Id == gebruikerId);
        }

        public bool VerlofDagenVanGebruikerExists(VerlofDagenVanGebruiker verlofDagen)
        {
            return _context.VerlofDagenVanGebruikers.Any(gebruiker1 => gebruiker1.Id == verlofDagen.Id);
        }

        public bool TijdslotenExistOpDatum(DateTime datum)
        {
            return _context.Tijdslot.Any(tijdslot => tijdslot.Tijdstip.Date == datum.Date);
        }
    }
}
