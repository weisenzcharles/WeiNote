using AOPStaticProxy.Model;
using AOPStaticProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPStaticProxy.Decorator
{

    /// <summary>
    /// 装饰器类。
    /// </summary>
    public class UserDecorator : IUserService
    {
        private readonly IUserService _userService;

        public UserDecorator(IUserService userService)
        {
            _userService = userService;
        }

        public void Register(User user)
        {
            Before();
            // 这里调用注册的方法，原有类里面的逻辑不会改变
            // 在逻辑前面和后面分别添加其他逻辑
            _userService.Register(user);

            After();
        }

        private void Before()
        {
            Console.WriteLine("注册之前的逻辑。");
        }

        private void After()
        {
            Console.WriteLine("注册之后的逻辑。");
        }


    }
}
