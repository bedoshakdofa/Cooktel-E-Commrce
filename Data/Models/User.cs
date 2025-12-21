using Cooktel_E_commrece.Data.Models.Enum;
using MimeKit.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Data.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }= Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string? PasswordHashed { get; set; }
        [Required]
        public Role UserRole { get; set; }
        public int? OTP_number { get; set; }
        public DateTime? ExpOTP { get; set; }
        public bool isActive { get; set; } = false;
        public DateTime? TimeToDelete { get; set; }

        public bool isDeleted { get; set; }=false;

        public ICollection<CardNumber> CardNumbers { get; set; }
        public ICollection<Reviews> Reviews { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public ICollection<Orders> Orders { get; set; }
        public Cart Cart { get; set; }

    }
}
