using Lekkerbek.Web.Context;
using Lekkerbek.Web.Controllers;
using Lekkerbek.Web.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Lekkerbek.Test.Controller
{
    [TestClass]
    public class BestellingsControllerTest
    {
        private readonly RoleManager<Role> _roleManager; 
        private readonly UserManager<Gebruiker> _userManager;

        [TestMethod]
        public void IndexTest()
        {

            // ARRANGE 
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseInMemoryDatabase("LekkerbekDb");
            IdentityContext _context = new IdentityContext(builder.Options);

            BestellingController bc = new BestellingController(_context, _roleManager, _userManager);

            // ACT 
            Task<IActionResult> result = bc.Index();

            // ASSERT 
            Assert.IsNotNull(result);

        }
        public void CreateTest()
        {

            // ARRANGE 

            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseInMemoryDatabase("LekkerbekDb");
            IdentityContext _context = new IdentityContext(builder.Options);

            BestellingController bc = new BestellingController(_context, null, null);
            // ACT 
            IActionResult result = bc.Create();

            // ASSERT 
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ViewResult);
            ViewResult viewResult = (ViewResult)result; 
        }
    }
}
