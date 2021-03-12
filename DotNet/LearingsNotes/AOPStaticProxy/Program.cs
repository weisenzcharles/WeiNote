using AOPStaticProxy.Decorator;
using AOPStaticProxy.Model;
using AOPStaticProxy.Proxy;
using AOPStaticProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPStaticProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // 实例化对象。
            IUserService userService = new UserService();
            //// 实例化装饰器类，并用上面的实例给构造方法传值。
            //var userDecorator = new UserDecorator(userService);
            //var user = new User { Name = "Charles", Password = "12345678" };
            //// 调用装饰器类的注册方法，相当于调用实例化对象的注册方法。
            //userDecorator.Register(user);

            //Console.ReadKey();




            //// 实例化对象
            //IUserService userService = new UserService();
            // 实例化装饰器类，并用上面的实例给构造方法传值
            var userProxy = new UserProxy();
            var user = new User { Name = "Charles", Password = "12345678" };
            // 调用装饰器类的注册方法，相当于调用实例化对象的注册方法
            userProxy.Register(user);

            Console.ReadKey();
        }
    }
}
