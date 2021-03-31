using AOPDynamicProxy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPDynamicProxy.Services
{
    /// <summary>
    /// 用户服务接口实现。
    /// </summary>
    public class UserService : MarshalByRefObject, IUserService
    {
        /// <summary>
        /// 注册用户。
        /// </summary>
        /// <param name="user">用户信息。</param>
        public void Register(User user)
        {
            Console.WriteLine($"{user.Name} 注册成功！");
        }
    }
}
