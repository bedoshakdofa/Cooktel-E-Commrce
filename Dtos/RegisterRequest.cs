using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(11)]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role UserRole { get; set; }

        [Required]
        public string Address { get; set; }
    }

}
