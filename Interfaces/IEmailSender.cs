namespace Cooktel_E_commrece.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string emailTo,string body,string subject);
    }
}
