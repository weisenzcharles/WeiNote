using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DotNetCodeFirst.Domain
{

    [Table("Categories")]
    public class Category
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        //[NotMapped]
        public virtual IList<MovieCategory> CategoryMovies { get; set; }
    }
}
