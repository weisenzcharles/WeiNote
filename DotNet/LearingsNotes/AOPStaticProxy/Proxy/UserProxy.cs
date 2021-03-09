using AOPStaticProxy.Model;
using AOPStaticProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPStaticProxy.Proxy
{
    public class UserProxy : IUserService
    {
        public void Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}
