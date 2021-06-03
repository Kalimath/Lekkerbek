using System;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLekkerbek
{
    [TestClass]
    public class BeoordelingTests
    {
        [TestMethod]
        public void InitializedBeoordelingSetsTotaalscore()
        {
            //ARRANGE
            Gebruiker nieuweGebruiker = new Gebruiker()
            {
                Id = 892529,
                UserName = "Ja",
                Adres = "Brussel",
                Geboortedatum = DateTime.Now.AddYears(-19),
                Getrouwheidsscore = 15,
                Email = "info@niks.be",
                IsProfessional = false,
                PasswordHash = "kroepoek"
            }; 

            //ACT
            Beoordeling recensie = new Beoordeling("niet lekker", "spreekt voor zich", 7.4,3.2,6.3,8.2, nieuweGebruiker.Id);

            //ASSERT
            Assert.AreEqual(6.275, recensie.TotaalScore);
        }

        [TestMethod]
        public void InitializedBeoordelingWithKlantIdReturnsCorrectValue()
        {
            //ARRANGE
            Gebruiker nieuweGebruiker = new Gebruiker()
            {
                Id = 892529,
                UserName = "Ja",
                Adres = "Brussel",
                Geboortedatum = DateTime.Now.AddYears(-19),
                Getrouwheidsscore = 15,
                Email = "info@niks.be",
                IsProfessional = false,
                PasswordHash = "kroepoek"
            };
            //ACT
            Beoordeling recensie = new Beoordeling("niet lekker", "spreekt voor zich", 7.4, 3.2, 6.3, 8.2, nieuweGebruiker.Id);

            //ASSERT
            Assert.AreEqual(892529, recensie.KlantId);

        }

        [TestMethod]
        public void InitializedBeoordelingWithTitelReturnsCorrectValue()
        {
            //ARRANGE
            Gebruiker nieuweGebruiker = new Gebruiker()
            {
                Id = 892529,
                UserName = "Ja",
                Adres = "Brussel",
                Geboortedatum = DateTime.Now.AddYears(-19),
                Getrouwheidsscore = 15,
                Email = "info@niks.be",
                IsProfessional = false,
                PasswordHash = "kroepoek"
            };
            //ACT
            Beoordeling recensie = new Beoordeling("niet lekker", "spreekt voor zich", 7.4, 3.2, 6.3, 8.2, nieuweGebruiker.Id);

            //ASSERT
            Assert.AreEqual("niet lekker", recensie.Titel);

        }

        [TestMethod]

        public void InitializedBeoordelingWithScorelijstNullThrowsError()
        {
            //ARRANGE
            Gebruiker nieuweGebruiker = new Gebruiker()
            {
                Id = 892529,
                UserName = "Ja",
                Adres = "Brussel",
                Geboortedatum = DateTime.Now.AddYears(-19),
                Getrouwheidsscore = 15,
                Email = "info@niks.be",
                IsProfessional = false,
                PasswordHash = "kroepoek"
            };

            //ACT
            //ASSERT
            Assert.ThrowsException<NullReferenceException>(() =>
                new Beoordeling("niet lekker", "spreekt voor zich", null, nieuweGebruiker.Id));
        }
    }

}
