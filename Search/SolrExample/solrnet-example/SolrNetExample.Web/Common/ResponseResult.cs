using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrNetExample.Web.Common
{
    /// <summary>
    /// 通用 Api 响应结果类。
    /// </summary>
    [Serializable]
    public class ResponseResult
    {
        #region Fields...

        #endregion

        #region Constructors...

        /// <summary>
        /// 初始化 <see cref="ResponseResult"/> 类的新实例。
        /// </summary>
        public ResponseResult() : this(ResponseStatus.SUCCEED, new object(), string.Empty)
        {

        }

        /// <summary>
        /// 使用响应状态值和响应错误信息初始化 <see cref="ResponseResult"/> 类的新实例。
        /// </summary>
        /// <param name="status">响应状态值。</param>
        /// <param name="error">响应错误信息。</param>
        public ResponseResult(int status, object extend, string error)
        {
            Status = status;
            Data = new object();
            Extend = extend;
            Error = error;
        }

        #endregion

        #region Properties...

        /// <summary>
        /// 获取或设置响应错误信息。
        /// </summary>

        /// <summary>
        /// 获取或设置响应状态值。
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 获取或设置响应成功的数据信息。
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 扩展。
        /// </summary>
        public object Extend { get; set; }

        /// <summary>
        /// 获取或设置响应错误信息。
        /// </summary>
        public string Error { get; set; }


        #endregion
    }

    /// <summary>
    /// 支持泛型的通用 Api 响应结果类。
    /// </summary>
    /// <typeparam name="T">指定的对象类型。</typeparam>
    public class ResponseResult<T> : ResponseResult
    {
        /// <summary>
        /// 获取或设置响应成功的数据信息。
        /// </summary>
        public new T Data { get; set; }

        /// <summary>
        /// 初始化 <see cref="ResponseResult{T}"/> 类的新实例。
        /// </summary>
        public ResponseResult(T data) : base()
        {
            Data = data;
        }
        /// <summary>
        /// 使用指定的参数初始化 <see cref="ResponseResult{T}"/> 类的新实例。
        /// </summary>
        public ResponseResult(int status, string error, T data)
        {
            Status = status;
            Error = error;
            Data = data;
        }
    }
}
