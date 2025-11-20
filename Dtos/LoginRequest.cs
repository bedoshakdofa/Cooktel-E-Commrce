using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
