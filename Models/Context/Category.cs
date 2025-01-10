using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Category")]
    public class Category : CommonAbstract
    {
        public Category()
        {
            this.News = new HashSet<News>();
            this.Posts = new HashSet<Posts>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Tên danh mục không được để trống!")]
        [StringLength(150)]
        public string Title { get; set; }

        public string Alias { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        [StringLength(150)]
        public string SeoTitle { get; set; }

        [StringLength(150)]
        public string SeoDescription { get; set; }

        [StringLength(150)]
        public string SeoKeywords { get; set; }
        public bool IsActive { get; set; }

        public int Position { get; set; }

        public ICollection<News> News { get; set; }
        public ICollection<Posts> Posts { get; set; }
    }
}