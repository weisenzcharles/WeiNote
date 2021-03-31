using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPDynamicProxy.Proxy
{
    /// <summary>
    /// 动态代理工厂类。
    /// </summary>
    public static class ProxyFactory
    {
        public static T Create<T>(Action before, Action after)
        {
            // 实例化被代理泛型对象。
            T instance = Activator.CreateInstance<T>();
            // 实例化动态代理，创建动态代理对象。
            var proxy = new DynamicProxy<T>(instance) { BeforeAction = before, AfterAction = after };
            // 返回透明代理对象。
            return (T)proxy.GetTransparentProxy();
        }
    }
}
