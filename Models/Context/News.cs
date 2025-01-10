using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Bạn không thể để trống tiêu để thông tin")]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Alias { get; set; }
        public string Description { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        public Guid CategoryId { get; set; }
        [StringLength(250)]
        public string SeoTitle { get; set; }
        [StringLength(500)]
        public string SeoDescription { get; set; }
        [StringLength(250)]
        public string SeoKeywords { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
    }
}