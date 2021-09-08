using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebMVC.Services
{
   
        public interface IIdentityService<T> 
        { 
            T Get(IPrincipal principal); //Get the User (Principal:term used in token for user information)
        }
    
}
