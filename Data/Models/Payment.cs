using Cooktel_E_commrece.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Data.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal totalAmount { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public PaymentStatus Status { get; set; }
        [Required]

        public PaymentType Type { get; set; }

        public Orders order_payment { get; set; }

    }
}
