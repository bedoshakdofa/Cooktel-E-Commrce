using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }

        [MaxLength(200)]
        public string token { get; set; }

        public DateTime ExpiresIn { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
