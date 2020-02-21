using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonServiceLocator;
using Microsoft.AspNetCore.Mvc;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNetExample.Web.Common;
using SolrNetExample.Web.Document;
using SolrNetExample.Web.Paging;

namespace SolrNetExample.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private ISolrOperations<PostDoc> solr;


        public SearchController()
        {
            solr = ServiceLocator.Current.GetInstance<ISolrOperations<PostDoc>>();
        }

        /// <summary>
        /// 增加索引。
        /// </summary>
        /// <returns></returns>
        [HttpGet("add")]
        public async Task<ResponseResult> Add()
        {
            // 同步添加文档
            solr.Add(
                new PostDoc()
                {
                    Id = 30001,
                    Name = "This SolrNet Name",
                    Title = "This SolrNet Title",
                    Excerpt = "This SolrNet Excerpt",
                    Content = "This SolrNet Content 30001",
                    PostDate = DateTime.Now
                }
            );
            // 异步添加文档
            await solr.AddAsync(
                new PostDoc()
                {
                    Id = 30002,
                    Name = "This SolrNet Name",
                    Title = "This SolrNet Title",
                    Excerpt = "This SolrNet Excerpt",
                    Content = "This SolrNet Content 30002",
                    PostDate = DateTime.Now
                }
            );

            ResponseHeader responseHeader = await solr.CommitAsync();
            ResponseResult response = new ResponseResult();
            if (responseHeader.Status == 0)
            {
                response.Status = ResponseStatus.SUCCEED;
            }
            return response;
        }


        /// <summary>
        /// 删除索引。
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> Delete()
        {
            // 使用文档 Id 删除
            await solr.DeleteAsync("300001");
            // 直接删除文档
            await solr.DeleteAsync(new PostDoc()
            {
                Id = 30002,
                Name = "This SolrNet Name",
                Title = "This SolrNet Title",
                Excerpt = "This SolrNet Excerpt",
                Content = "This SolrNet Content 30002",
                PostDate = DateTime.Now
            });
            // 提交
            ResponseHeader responseHeader = await solr.CommitAsync();
            ResponseResult response = new ResponseResult();
            if (responseHeader.Status == 0)
            {
                response.Status = ResponseStatus.SUCCEED;
            }
            return response;
        }


        /// <summary>
        /// 查询并排序。
        /// </summary>
        /// <returns></returns>
        [HttpGet("ordering")]
        public async Task<IActionResult> QueryOrdering()
        {
            // 排序
            ICollection<SortOrder> sortOrders = new List<SortOrder>() {
                new SortOrder("id", Order.DESC)
            };
            // 使用查询条件并排序
            SolrQueryResults<PostDoc> docs = await solr.QueryAsync("post_title:索尼", sortOrders);
            return Ok(new ResponseResult<SolrQueryResults<PostDoc>>(ResponseStatus.SUCCEED, string.Empty, docs));
        }

        /// <summary>
        /// 分页查询。
        /// </summary>
        /// <returns></returns>
        [HttpGet("query/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> QueryPagingAsync(int pageIndex = 0, int pageSize = 10)
        {

            // 使用条件查询
            SolrQueryResults<PostDoc> posts = solr.Query(new SolrQueryByField("id", "30000"));

            // 高级查询
            SolrQuery solrQuery = new SolrQuery("苹果");
            QueryOptions queryOptions = new QueryOptions
            {
                // 高亮关键字
                Highlight = new HighlightingParameters
                {
                    Fields = new List<string> { "post_title" },
                    BeforeTerm = "<font color='red'><b>",
                    AfterTerm = "</b></font>"
                },
                // 分页
                StartOrCursor = new StartOrCursor.Start(pageIndex * pageSize),
                Rows = pageSize
            };
            SolrQueryResults<PostDoc> docs = await solr.QueryAsync(solrQuery, queryOptions);
            var highlights = docs.Highlights;
            return Ok(new PagedList<PostDoc>(ResponseStatus.SUCCEED, string.Empty, docs, pageIndex, pageSize, docs.NumFound, highlights));
        }

    }
}