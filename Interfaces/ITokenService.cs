using Cooktel_E_commrece.Data.Models;

namespace Cooktel_E_commrece.Interfaces
{
    public interface ITokenService
    {
        string GetAccessToken(User user);

    }
}
