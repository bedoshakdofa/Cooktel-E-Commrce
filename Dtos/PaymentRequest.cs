using Cooktel_E_commrece.Data.Models.Enum;

namespace Cooktel_E_commrece.Dtos
{
    public class PaymentRequest
    {
        public PaymentType type {  get; set; } 

        public int OrderId { get; set; }
    }
}
