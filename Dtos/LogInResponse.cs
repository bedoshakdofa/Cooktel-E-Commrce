namespace Cooktel_E_commrece.Dtos
{
    public class LogInResponse
    {
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public string refreshToken { get; set; }
    }
}
