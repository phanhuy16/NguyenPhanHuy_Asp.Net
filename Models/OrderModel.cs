
using System.ComponentModel.DataAnnotations;

namespace NguyenPhanHuy_2122110062.Models
{
    public class OrderModel
    {
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
        public int TypePayment { get; set; }
    }
}