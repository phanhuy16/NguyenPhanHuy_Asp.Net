using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Statistical")]
    public class Statistical
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public long NumberOfVisits { get; set; }
    }
}