using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter name!")]
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters!")]
        public string Name { get; set; }
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters!")]
        public string Email { get; set; }
        public string Website { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}