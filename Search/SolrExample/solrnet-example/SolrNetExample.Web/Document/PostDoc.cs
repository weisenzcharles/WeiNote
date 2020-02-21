using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrNetExample.Web.Document
{

    /// <summary>
    /// 文章索引文档。
    /// </summary>
    [Serializable]
    public class PostDoc
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }

        [SolrField("post_title")]
        public string Title { get; set; }

        [SolrField("post_name")]
        public string Name { get; set; }

        [SolrField("post_excerpt")]
        public string Excerpt { get; set; }

        [SolrField("post_content")]
        public string Content { get; set; }

        [SolrField("post_date")]
        public DateTime PostDate { get; set; }
    }
}
