using Newtonsoft.Json;
using SolrNetExample.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SolrNetExample.Web.Paging
{
    /// <summary>
    /// 表示一个分页列表对象。
    /// </summary>
    /// <typeparam name="T">分页的实体对象。</typeparam>
    [Serializable]
    public class PagedList<T> : ResponseResult, IPagedList<T>
    {
        /// <summary>
        /// 初始化一个包含支持查询的数据源的 <see cref="PagedList"/> 类的新实例。
        /// </summary>
        /// <param name="source">指定的数据源。</param>
        /// <param name="pageIndex">指定的分页索引。</param>
        /// <param name="pageSize">指定的分页大小。</param>
        public PagedList(int status, string error, IQueryable<T> source, int pageIndex, int pageSize, object extend = null) : base(status, extend, error)
        {
            Data = new List<T>();
            //Status = status;
            //Error = error;

            int total = source.Count();
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            Data.AddRange(source.ToList());
        }

        /// <summary>
        /// 初始化一个包含支持索引的数据源的 <see cref="PagedList"/> 类的新实例。
        /// </summary>
        /// <param name="source">指定的数据源。</param>
        /// <param name="pageIndex">指定的分页索引。</param>
        /// <param name="pageSize">指定的分页大小。</param>
        public PagedList(int status, string error, IList<T> source, int pageIndex, int pageSize, int totalCount, object extend = null) : base(status, extend, error)
        {
            Data = new List<T>();
            //Status = status;
            //Error = error;

            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            Data.AddRange(source.ToList());
        }

        /// <summary>
        /// 初始化一个包含支持枚举的数据源的 <see cref="PagedList"/> 类的新实例。
        /// </summary>
        /// <param name="source">指定的数据源。</param>
        /// <param name="pageIndex">指定的分页索引。</param>
        /// <param name="pageSize">指定的分页大小。</param>
        /// <param name="totalCount">记录总数。</param>
        public PagedList(int status, string error, IEnumerable<T> source, int pageIndex, int pageSize, int totalCount, object extend = null) : base(status, extend, error)
        {
            Data = new List<T>();

            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            Data.AddRange(source);
        }

        /// <summary>
        /// 数据列表。
        /// </summary>
        public new List<T> Data { get; private set; }

        /// <summary>
        /// 当前页面的索引（从 0 开始）。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 当前页面显示的项目数量。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 项目的记录总数。
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 页面的记录总数。
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// 是否有在当前页面之前的页面。
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        /// <summary>
        /// 是否有在当前页面之后的页面。
        /// </summary>
        public bool HasNextPage
        {
            get { return PageIndex + 1 < TotalPages; }
        }

    }
}
