using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Services
{
    public class ServiceException : ArgumentException
    {
        public ServiceException(string message):base(message)
        {
            
        }
    }
}
