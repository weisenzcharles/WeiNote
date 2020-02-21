using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrNetExample.Web.Common
{
    /// <summary>
    /// Api 响应状态定义类。
    /// </summary>
    public static class ResponseStatus
    {
        /// <summary>
        /// 发生错误。
        /// </summary>
        public static readonly int ERROR = 0;

        /// <summary>
        /// 成功。
        /// </summary>
        public static readonly int SUCCEED = 1;
    }
}
