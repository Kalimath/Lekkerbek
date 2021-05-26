using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Services
{
    public interface ICategorieService
    {
        public Categorie GetCategorie(string categorieNaam);
        public Task AddCategorie(Categorie categorie);
        public ICollection<Categorie> GetCategorieen();
        public Task UpdateCategorie(Categorie updatedCategorie);
        public Task DeleteCategorie(string categorieNaam);
        public bool CategorieExists(Categorie categorie);
    }
}
