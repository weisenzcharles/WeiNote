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
        private IUserService _userService;

        public UserProxy()
        {
            _userService = new UserService();
        }

        public void Register(User user)
        {
            Before();

            _userService.Register(user);

            After();
        }

        private void Before()
        {
            Console.WriteLine("注册之前的逻辑！");
        }

        private void After()
        {
            Console.WriteLine("注册之后的逻辑！");
        }
    }
}
