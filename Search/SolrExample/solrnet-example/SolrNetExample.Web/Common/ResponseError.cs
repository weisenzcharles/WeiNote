using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrNetExample.Web.Common
{
    /// <summary>
    /// Api 响应错误信息定义类。
    /// </summary>
    public static class ResponseError
    {
        /// <summary>
        /// 请求成功。
        /// </summary>
        public static readonly string REQUEST_SUCCESS = "REQUEST_SUCCESS: 请求成功。";
        /// <summary>
        /// 请求失败。
        /// </summary>
        public static readonly string REQUEST_FAILED = "REQUEST_FAILED: 请求失败。";
    }
}
