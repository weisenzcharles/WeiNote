using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DotNetCodeFirst.Domain
{
    /// <summary>
    /// 电影分类关系。
    /// </summary>
    [Table("MovieCategories")]
    public class MovieCategory
    {
        ///// <summary>
        ///// 标识。
        ///// </summary>
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        public int MovieId { get; set; }

        public int CategoryId { get; set; }



        public virtual Category Category { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
