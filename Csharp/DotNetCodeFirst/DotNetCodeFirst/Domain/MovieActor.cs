using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCodeFirst.Domain
{
    /// <summary>
    /// 电影演员关系。
    /// </summary>
    [Table("MovieActors")]
    public class MovieActor
    {
        ///// <summary>
        ///// 标识。
        ///// </summary>
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        public int MovieId { get; set; }

        public int ActorId { get; set; }



        public virtual Actor Actor { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
