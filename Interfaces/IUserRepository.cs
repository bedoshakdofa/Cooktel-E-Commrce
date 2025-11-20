using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Cooktel_E_commrece.Interfaces
{
    public interface IUserRepository
    {
        Task<User>GetOne(Guid id);
        Task<bool> SaveAllChanges();
        void Delete(User user);

        void Update(JsonPatchDocument<UserDto> PatchUserDto, User user, ModelStateDictionary ModelState);
    }
}
