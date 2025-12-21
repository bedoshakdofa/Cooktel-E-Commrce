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
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;

        public int? PaymentId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment? payment { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
