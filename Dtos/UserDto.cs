using Cooktel_E_commrece.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
   
        [Required]
        public string FirstName { get; set; }
   
        [Required]
        public string LastName { get; set; }
        [Required]
    
        public string PhoneNumber { get; set; }
        [Required]
   
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public Role UserRole { get; set; }
    }
}
