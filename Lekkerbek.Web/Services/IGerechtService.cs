using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lekkerbek.Web.Models;

namespace Lekkerbek.Web.Services
{
    interface IGerechtService
    {
        public Gerecht GetGerecht(string gerechtNaam);
        public Task AddGerecht(Gerecht gerecht);
        public ICollection<Gerecht> GetGerechten();
        public Task UpdateGerecht(Gerecht updatedGerecht);
        public Task DeleteGerecht(string gerechtNaam);
        public bool GerechtExists(Gerecht gerecht);
    }
}
