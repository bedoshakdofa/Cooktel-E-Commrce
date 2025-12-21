using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Helper
{
    public class MailSettings
    {
        [Required]
        public string Host {  get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string DisplayName {  get; set; }
    }
}
