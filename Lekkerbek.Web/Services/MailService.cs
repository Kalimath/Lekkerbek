using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;
using Lekkerbek.Web.Models.Identity;

namespace Lekkerbek.Web.Services
{
    public class MailService : IMailService
    {
        public static string AdresZender = "gip.sender.team11@outlook.com";
        public async Task SendPromotie(Gebruiker klant, bool menuInBijlage)
        {
            try
            {
                var message = ConvertMailMessage(klant.Email, "Lekkerbek Promotie");
                string textEmail = await File.ReadAllTextAsync("ExterneBestanden/EmailSjabloon.txt");
                string korting = "";
                string voorkeursgerechten = "";
                string aanspreking = "";
                if (klant.Bestellingen.Count % 2 == 0 && klant.Bestellingen.Count != 0 && klant.Bestellingen.Count != 1)
                {
                    korting = "Op uw eerstvolgende bestelling ontvangt u een korting";
                }
                switch (klant.Geslacht)
                {
                    case "Man":
                        aanspreking = "heer ";
                        break;
                    case "Vrouw":
                        aanspreking = "mevrouw ";
                        break;
                    default:
                        aanspreking = "";
                        break;
                }
                foreach (Gerecht g in klant.Voorkeursgerechten)
                {
                    voorkeursgerechten += "<p> - " + g.Naam + "</p>\n";
                }
                textEmail = string.Format(textEmail, aanspreking, klant.UserName, korting, voorkeursgerechten);
                message.Body = textEmail;
                message.IsBodyHtml = true;
                if (menuInBijlage)
                {
                    message.Attachments.Add(new Attachment("ExterneBestanden/MenukaartLekkerbek.pdf"));
                }

                var smtp = new SmtpClient("smtp.outlook.com");
                smtp.Credentials = new NetworkCredential(AdresZender, "Lekkerbek123");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //stuurt email van klacht naar alle admins en een bevestiging naar de desbetreffende klant
        public async Task SendKlacht(Klacht klacht, List<Gebruiker> adminGebruikers)
        {

            try
            {
                var klant = klacht.Klant;
                var message = ConvertMailMessage(klant.Email, "Klacht " + klacht.Id +": "+ klacht.Onderwerp);
                string textEmail = await File.ReadAllTextAsync("ExterneBestanden/EmailKlacht.txt");

                    textEmail = string.Format(textEmail, klacht.Omschrijving+'\n'+klacht.Tijdstip, klant.UserName);
                    message.Body = textEmail;
                    message.IsBodyHtml = true;

                    var smtp = new SmtpClient("smtp.outlook.com");
                    smtp.Credentials = new NetworkCredential(AdresZender, "Lekkerbek123");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    
                    await SendKlachtBevestiging(klacht);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SendKlachtBevestiging(Klacht klacht)
        {
            try
            {
                var klant = klacht.Klant;
                var message = ConvertMailMessage(klant.Email, "Bevestiging van uw klacht met id: " + klacht.Id);
                string textEmail = await File.ReadAllTextAsync("ExterneBestanden/EmailKlachtBevestiging.txt");
                string aanspreking = "";
                switch (klant.Geslacht)
                {
                    case "Man":
                        aanspreking = "heer ";
                        break;
                    case "Vrouw":
                        aanspreking = "mevrouw ";
                        break;
                    default:
                        aanspreking = "";
                        break;
                }
                textEmail = string.Format(textEmail, aanspreking, klacht.Id);
                message.Body = textEmail;
                message.IsBodyHtml = true;

                var smtp = new SmtpClient("smtp.outlook.com");
                smtp.Credentials = new NetworkCredential(AdresZender, "Lekkerbek123");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public MailMessage ConvertMailMessage(string adresOntvanger, string onderwerp)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(adresOntvanger));
            message.From = new MailAddress(AdresZender);
            message.Subject = onderwerp;

            return message;
        }
    }
}
