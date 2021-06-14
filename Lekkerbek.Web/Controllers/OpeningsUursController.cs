﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Services;
using Lekkerbek.Web.ViewModels.OpeningsUur;
using Microsoft.AspNetCore.Authorization;

namespace Lekkerbek.Web.Controllers
{
    [Authorize]
    public class OpeningsUursController : Controller
    {
        private readonly IKalenderService _kalenderService;

        public OpeningsUursController(IKalenderService kalenderService)
        {
            _kalenderService = kalenderService;
        }

        // GET: OpeningsUurs
        [AllowAnonymous]
        public  IActionResult Index()
        {
            var model = from c in _kalenderService.GetOpeningsUren()
                        select new OpeningsUurViewModel()
                        {
                            Id = c.Id,
                            Dag = c.Dag,
                            Uur = c.Uur
                        };
            return View(model);
        }

        // GET: OpeningsUurs/Details/5
        [Authorize(Roles = "Admin")]
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

            return View(openingsUur);
        }

        // GET: OpeningsUurs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OpeningsUurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Dag,Uur,Startuur,Sluitingsuur,Datum")] OpeningsUur openingsUur)
        {
            if (ModelState.IsValid)
            {
                await _kalenderService.AddOpeningsUur(openingsUur);
                return RedirectToAction(nameof(Index));
            }
            return View(openingsUur);
        }

        // GET: OpeningsUurs/Edit/5
        [Authorize(Roles = "Admin")]
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
        }

        // POST: OpeningsUurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dag,Uur,Datum")] OpeningsUur openingsUur)
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
        }

        /*// GET: OpeningsUurs/Delete/5
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
