using NguyenPhanHuy_2122110062.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Brand")]
    public class Brand : CommonAbstract
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        [StringLength(250)]
        public string SeoTitle { get; set; }

        [StringLength(500)]
        public string SeoDescription { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }
        public virtual Product Product { get; set; }
    }
}