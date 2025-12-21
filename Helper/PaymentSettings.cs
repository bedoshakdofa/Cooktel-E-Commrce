using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Helper
{
    public class PaymentSettings
    {
        [Required]
        public string APIKey { get; set; }
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public string SecretKey { get; set; }
        [Required]
        public string CardIntegrationId { get; set; }
        [Required]
        public string HMAC { get; set; }
    }
}
