using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolrNetExample.Web.Paging
{
    /// <summary>
    /// 分页列表接口。
    /// </summary>
    public interface IPagedList<T> 
    {
        /// <summary>
        /// 当前页面的索引（从 0 开始）。
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 当前页面显示的项目数量。
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 项目的记录总数。
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 页面的记录总数。
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 是否有在当前页面之前的页面。
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有在当前页面之后的页面。
        /// </summary>
        bool HasNextPage { get; }
    }
}
