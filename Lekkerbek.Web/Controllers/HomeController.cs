using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.ViewModels.OpeningsUur;
using Microsoft.EntityFrameworkCore;
using Lekkerbek.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Lekkerbek.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private IdentityContext _context;
        public HomeController(IdentityContext context)
        {
            _context = context;
        }

        

        // GET: OpeningsUurs
        public IActionResult Index()
        {
            var model = from c in _context.OpeningsUren
                        select new OpeningsUurViewModel()
                        {
                            Id = c.Id,
                            Dag = c.Dag,
                            Uur = c.ToString()
                        };
            return View(model);
        }
    }
}
