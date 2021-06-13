using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Context;
using Lekkerbek.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lekkerbek.Web.Services
{
    public class CategorieService : ICategorieService
    {
        private IdentityContext _context;

        public CategorieService(IdentityContext context)
        {
            _context = context;
        }

        public Categorie GetCategorie(string categorieNaam)
        {
            try
            {
                return _context.Categorie.FirstOrDefault(categorie => categorie.Naam.Trim().Equals(categorieNaam.Trim()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task AddCategorie(Categorie categorie)
        {
            try
            {
                if (!CategorieExists(categorie))
                {
                    await _context.Categorie.AddAsync(categorie);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ServiceException("Categorie bestaat al");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public ICollection<Categorie> GetCategorieen()
        {
            try
            {
                return _context.Categorie.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task UpdateCategorie(Categorie updatedCategorie)
        {
            try
            {
                _context.Categorie.Update(updatedCategorie);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public async Task DeleteCategorie(string categorieNaam)
        {
            try
            {
                using Categorie categorie = GetCategorie(categorieNaam);
                _context.Categorie.Remove(categorie);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServiceException(e.Message);
            }
        }

        public bool CategorieExists(Categorie categorie)
        {
            bool result = false;
            try
            {
                if (_context.Categorie.Any(categorie1 => categorie1.Naam.Trim().Equals(categorie.Naam.Trim())))
                {
                    result = true;
                }
            }
            catch (Exception ignore)
            {
                // ignored
            }

            return result;
        }
    }
}
