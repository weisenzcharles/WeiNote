using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCodeFirst.Domain
{
    [Table("Movies")]
    public class Movie
    {
        /// <summary>
        /// 唯一标识。
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 长度。
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 预览图片。
        /// </summary>
        public string PreviewImage { get; set; }

        /// <summary>
        /// 导演。
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// 制作。
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// 上映日期。
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// 演员。
        /// </summary>
        [NotMapped]
        public virtual IList<Actor> Actors { get; set; }

        /// <summary>
        /// 电影演员关系。
        /// </summary>
        //[NotMapped]
        public virtual IList<MovieActor> MovieActors { get; set; }

        /// <summary>
        /// 电影演员关系。
        /// </summary>
        //[NotMapped]
        public virtual IList<MovieCategory> MovieCategories { get; set; }

        /// <summary>
        /// 分类。
        /// </summary>
        [NotMapped]
        public virtual IList<Category> Categories { get; set; }

        /// <summary>
        /// 图片。
        /// </summary>
        //[NotMapped]
        public virtual IList<Screenshot> Screenshots { get; set; }

    }
}
