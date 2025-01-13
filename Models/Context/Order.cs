using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NguyenPhanHuy_2122110062.Models.Context
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn!")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhâp số điện thoại của bạn!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ nhận hàng!")]
        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public int Status { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}