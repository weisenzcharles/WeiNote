using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCodeFirst.Domain
{
    [Table("Actors")]
    public class Actor
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
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 电影。
        /// </summary>
        [NotMapped]
        public virtual IList<Movie> Movies { get; set; }

        /// <summary>
        /// 演员电影关系。
        /// </summary>
        //[NotMapped]
        public virtual IList<MovieActor> ActorMovies { get; set; }
    }
}
