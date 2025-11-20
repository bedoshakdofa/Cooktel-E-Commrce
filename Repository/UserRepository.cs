using AutoMapper;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cooktel_E_commrece.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(AppDbContext context , IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public void Delete(User user)
        {
            user.isDeleted= true;
            user.TimeToDelete= DateTime.UtcNow.AddDays(60);
        }

        public async Task<User> GetOne(Guid id)
        {
            return await _context.users.FindAsync(id);
        }

        public void Update(JsonPatchDocument<UserDto> PatchUserDto, User user, ModelStateDictionary ModelState)
        {
            var userDto = _mapper.Map<UserDto>(user);

            PatchUserDto.ApplyTo(userDto, ModelState);

            _mapper.Map(userDto, user);
        }

        public async Task<bool> SaveAllChanges()
        {
        
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

    }

}
