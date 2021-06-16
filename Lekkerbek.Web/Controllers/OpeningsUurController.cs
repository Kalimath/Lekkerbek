using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Kalender;
using Lekkerbek.Web.Services;
using Lekkerbek.Web.ViewModels.OpeningsUur;
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OpeningsUurController : Controller
    {
        private readonly IKalenderService _kalenderService;
        private readonly IGebruikerService _gebruikerService;

        public OpeningsUurController(IKalenderService kalenderService, IGebruikerService gebruikerService)
        {
            _kalenderService = kalenderService;
            _gebruikerService = gebruikerService;
        }

        // GET: OpeningsUur
        public  IActionResult Index()
        {
            var model = _kalenderService.GetOpeningsUren().AsEnumerable();
                        
            return View(model);
        }

        // GET: OpeningsUur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingsUur = _kalenderService.GetOpeningsUren()
                .FirstOrDefault(m => m.Id == id);
            if (openingsUur == null)
            {
                return NotFound();
            }

            return View();
        }

        // GET: OpeningsUur/Details/5
        public async Task<IActionResult> DagInfo(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var openingsUren = _kalenderService.GetOpeningsUur(id);

            if(openingsUren!=null && !openingsUren.IsGesloten)
            {
                var datum = openingsUren.Startuur.Date;
                var tijdslotenVanDag = _kalenderService.GetTijdslotenOpDag(datum);
            
                DagInfoViewModel vm = new DagInfoViewModel()
                {
                    OpeningsUur = openingsUren,
                    AantalKoksBeschikbaar = _kalenderService.AantalKoksBeschikbaarOpDatum(datum),
                    KoksVakantieOpDag = _kalenderService.GetGebruikersMetVerlofOpDatum(datum),
                    KoksZiekOpDag = _kalenderService.GetGebruikersZiekOpDatum(datum),
                    Tijdsloten = tijdslotenVanDag
                };
                return View(vm);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        //List van ale Koks
        [Authorize(Roles = "Admin")]
        public IActionResult Koks()
        {
            var vm = from g in _gebruikerService.GetGebruikersMetRolKok()
                             select new AlleKoksOpDagViewModel()
                             {
                                 Gebruikersnaam = g.NormalizedUserName,
                                 Rol = g.UserName
                             };
            return View(vm);
        }

        // GET: OpeningsUur/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OpeningsUur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dag,Uur,IsGesloten,Startuur,SluitingsUur")] OpeningsUur openingsUur)
        {
            if (ModelState.IsValid && openingsUur.Startuur.Date == openingsUur.SluitingsUur.Date)
            {
                await _kalenderService.AddOpeningsUur(openingsUur);
                return RedirectToAction(nameof(Index));
            }
            return View(openingsUur);
        }
        /*
        // GET: OpeningsUur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var openingsUur = _kalenderService.GetOpeningsUren()
                .FirstOrDefault(m => m.Id == id);
            if (openingsUur == null)
            {
                return NotFound();
            }
            return View(openingsUur);
        }*/
        
        public IActionResult IsZiekOfOpVerlof(int id, DateTime dateTime)
        {
            /*var update = from g in _kalenderService.GetGebruikersZiekOpDatum(dateTime)
                         where id == g.Id
                         select new RegistreerViewModel()
                         {
                             Id = g.Id
                         };
            _kalenderService.UpdateZiekteDagenVanGebruiker((ZiekteDagenVanGebruiker)update);
            var kk = from h in _kalenderService.GetAlleTijdsloten()
                     select new RegistreerViewModel()
                     {
                         IsVrij = h.IsVrij
                     };
            _kalenderService.UpdateZiekteDagenVanGebruiker((ZiekteDagenVanGebruiker)kk);

            var update1 = from t in _kalenderService.GetAlleTijdsloten()
                          where id == t.Id
                          select t.IsVrij == false;

            var update = from k in _gebruikerService.GetGebruikersMetRolKok()
                         where id == k.Id
                         select (from g in _kalenderService.GetAlleTijdsloten()
                                         select g.IsVrij == false);

            /*var deletedag = from d in _gebruikerService.GetGebruikersMetRolKok()
                            where id == d.Id
                            select (from dag in _kalenderService.get
                                    select dag);
            object p = deletedag.Count--;*/
            var op = _kalenderService.GetOpeningsUur(id);
            var date = op.Startuur;
            ZiekteDagenVanGebruiker ziek = new ZiekteDagenVanGebruiker(id);
            //ziek.Dagen = date;

            _kalenderService.AantalKoksBeschikbaarOpDatum(date);
            _kalenderService.AddZiekteDagenVanGebruiker(ziek);


            return RedirectToAction(nameof(Koks));
        }
        /*
        // POST: OpeningsUur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dag,Uur,IsGesloten,Startuur,SluitingsUur")] OpeningsUur openingsUur)
        {
            if (id != openingsUur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _kalenderService.UpdateOpeningsUren(openingsUur);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(openingsUur);
        }*/

        /*// GET: OpeningsUur/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var openingsUur = _kalenderService.GetOpeningsUren()
                .FirstOrDefault(m => m.Id == id);
            if (openingsUur == null)
            {
                return NotFound();
            }

            return View(openingsUur);
        }

        // POST: OpeningsUurs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var openingsUur = _kalenderService.GetOpeningsUren()
                .FirstOrDefault(m => m.Id == id);
            _kalenderService.de
            return RedirectToAction(nameof(Index));
        }*/
    }
}
