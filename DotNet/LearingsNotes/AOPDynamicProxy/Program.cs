using AOPDynamicProxy.Model;
using AOPDynamicProxy.Proxy;
using AOPDynamicProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPDynamicProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // 调用动态代理工厂类创建动态代理对象，传递 UserService，并且传递两个委托。
            var userService = ProxyFactory.Create<UserService>(
                before: () =>
            {
                Console.WriteLine("注册之前！");
            },
                after: () =>
            {
                Console.WriteLine("注册之后！");
            });

            User user = new User()
            {
                Name = "张三",
                Password = "123456"
            };
            // 调用注册方法
            userService.Register(user);

            Console.ReadKey();
        }
    }
}
