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
using Lekkerbek.Web.Services;
using Microsoft.AspNetCore.Http;

namespace Lekkerbek.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private IKalenderService _kalenderService;

        public HomeController(IdentityContext context, IKalenderService kalenderService)
        {
            _kalenderService = kalenderService;
        }

        

        // GET: OpeningsUurs
        public IActionResult Index()
        {
            var model = _kalenderService.GetOpeningsUren().AsEnumerable();

            return View(model);
        }
    }
}
