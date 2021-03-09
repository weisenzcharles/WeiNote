using AOPStaticProxy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPStaticProxy.Services
{

    /// <summary>
    /// 用户服务接口实现。
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 注册用户。
        /// </summary>
        /// <param name="user">用户信息。</param>
        void Register(User user);
    }
}
