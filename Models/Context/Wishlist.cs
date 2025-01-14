using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Wishlist")]
    public class Wishlist
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Product Product { get; set; }
    }
}