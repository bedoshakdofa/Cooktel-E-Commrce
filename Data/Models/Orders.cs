using Cooktel_E_commrece.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class Orders
    {
        [Key]
        public int Order_ID { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        DateTime CreatedDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment payment { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
