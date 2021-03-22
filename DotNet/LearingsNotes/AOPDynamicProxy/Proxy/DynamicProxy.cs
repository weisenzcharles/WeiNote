using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace AOPDynamicProxy.Proxy
{
    public class DynamicProxy<T> : RealProxy
    {
        private readonly T _target;

        /// <summary>
        /// 执行之前。
        /// </summary>
        public Action BeforeAction { get; set; }

        /// <summary>
        /// 执行之后。
        /// </summary>
        public Action AfterAction { get; set; }

        /// <summary>
        /// 使用指定的类型初始化 <see cref="DynamicProxy"/> 类的新实例。
        /// </summary>
        /// <param name="target">被代理泛型类。</param>
        public DynamicProxy(T target) : base(typeof(T))
        {
            _target = target;
        }

        /// <summary>
        /// 代理类调用方法。
        /// </summary>
        /// <param name="msg">要执行的方法。</param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            var reqMsg = msg as IMethodCallMessage;
            var target = _target as MarshalByRefObject;

            BeforeAction();

            // 这里才真正去执行代理类里面的方法，target 表示被代理的对象，reqMsg 表示要执行的方法。
            var result = RemotingServices.ExecuteMessage(target, reqMsg);

            AfterAction();

            return result;
        }

    }
}
