using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCodeFirst.Domain
{
    [Table("Screenshots")]
    public class Screenshot
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 电影标识。
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// 图片地址。
        /// </summary>
        public string Url { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
