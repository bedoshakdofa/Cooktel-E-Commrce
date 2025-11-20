using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Cooktel_E_commrece.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user=await _userRepository.GetOne(Guid.Parse(userId));
            
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPatch("update")]

        public async Task<ActionResult<UserDto>> UpdateUser([FromBody] JsonPatchDocument<UserDto> PatchUserDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userRepository.GetOne(Guid.Parse(userId));

            if (user == null)
                return NotFound("no user with this id");

           _userRepository.Update(PatchUserDto, user,ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userRepository.SaveAllChanges())
                return Ok("user updated successfully");
            return BadRequest("can't save to database");

        }

        [HttpDelete("Delete")]

        public async Task<ActionResult> DeleteUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user=await _userRepository.GetOne(Guid.Parse(userId));

            if (user == null) return NotFound("can't find user with this id");

            _userRepository.Delete(user);

            await _userRepository.SaveAllChanges();
            return Ok("your account has been diactivated to reactivate your account logIn again with 60 days");
        }
    }
}
