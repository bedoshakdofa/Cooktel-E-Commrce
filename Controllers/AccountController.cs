using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Cooktel_E_commrece.Controllers
{
    
    [ApiController]
    [Route("api/{Controller}")]
    public class AccountController : ControllerBase
    {
        
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController( IEmailSender emailSender, AppDbContext context, ILogger<AccountController> logger, ITokenService service)
        {
            _emailSender = emailSender;
            _context = context;
            _tokenService=service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invaild Input");
            }
            if (await checkUserExist(registerDto.EmailAddress))
            {
                return BadRequest("User Exist before");
            }

            var Random = new Random();
            registerDto.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                EmailAddress = registerDto.EmailAddress.ToLower(),
                PasswordHashed = registerDto.Password,
                Address = registerDto.Address,
                PhoneNumber = registerDto.PhoneNumber,
                UserRole = registerDto.UserRole,
                OTP_number = Random.Next(100000, 999999),
                ExpOTP = DateTime.UtcNow.AddMinutes(10),
            };

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("" +
                "Subject: Your Verification Code (OTP)\r\n\r\n" +
                $"Hello {registerDto.UserName},\r\n\r\n" +
                $"Your verification code is: {newUser.OTP_number}" +
                $"\r\n\r\nThis code will expire in 10 minutes.  " +
                $"\r\nIf you did not request this code, please ignore this email.\r\n\r\nThank you");


            await _emailSender.SendEmail(registerDto.EmailAddress, stringBuilder.ToString(), "OTP verfication");

            _context.users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser.EmailAddress);
        
        }
        [HttpPost("Verfiy")]
        public async Task<ActionResult> VerfiyUser([FromBody] VerfiyDto verfiyDto)
        {

            IQueryable<User> query = _context.users.AsQueryable();

            var user= await query.FirstOrDefaultAsync(x=>x.EmailAddress== verfiyDto.email.ToLower()&&!x.isActive);

            if (user == null) return NotFound("Can't find the user");

            if (verfiyDto.OTP!=user.OTP_number|| user.ExpOTP < DateTime.UtcNow)
            {
                return BadRequest("invaild or expired OTP Input");
            }

            user.isActive = true;
            user.OTP_number = null;
            user.ExpOTP=null;
            _context.users.Update(user);

            await _context.SaveChangesAsync();
            return Ok("your account verfied");

        }
        [HttpPost("resendOTP")]
        public async Task<ActionResult>ResndOTP([FromBody] string email)
        {
            IQueryable<User> query= _context.users.AsQueryable();
            var user = await query.FirstOrDefaultAsync(x => x.EmailAddress == email.ToLower() && !x.isActive);

            if (user == null) return NotFound("please register");

            var random = new Random();
            int otp = random.Next(10000, 99999);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("" +
                $"Hello {user.UserName},\r\n\r\n" +
                $"Your verification code is: {otp}" +
                $"\r\n\r\nThis code will expire in 10 minutes.  " +
                $"\r\nIf you did not request this code, please ignore this email.\r\n\r\nThank you");


            await _emailSender.SendEmail(user.EmailAddress, stringBuilder.ToString(), "OTP verfication");

            user.OTP_number=otp;
            user.ExpOTP = DateTime.UtcNow.AddMinutes(10);
            await _context.SaveChangesAsync();
            return Ok("Verfication Code has been sent");
        }

        [HttpPost("login")]

        public async Task<ActionResult<LogInResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Invaild Input");

            IQueryable<User> query = _context.users.AsQueryable();
            var user = await query.FirstOrDefaultAsync(x => x.EmailAddress == loginRequest.email.ToLower() && x.isActive);

            if (user == null) return NotFound("Invaild Email or Password");

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.password, user.PasswordHashed))
                return Unauthorized("Invaild Email Or Password");

            if (user.isDeleted)
            {
                user.isDeleted = false;
                user.TimeToDelete = null;
            }

            var token = _tokenService.GetAccessToken(user);

            var RefreshToken = new RefreshToken
            {
                ExpiresIn = DateTime.UtcNow.AddDays(10),
                token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                UserId = user.UserID
            };

            _context.RefreshToken.Add(RefreshToken);
            await _context.SaveChangesAsync();

            return new LogInResponse
            {
                JwtToken = token,
                Username = user.UserName,
               refreshToken=RefreshToken.token,
            };

        }

        [HttpPost("LoginWithToken")]
        public async Task<ActionResult<LogInResponse>> LoginWithRefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("please send a token");

            var token = await _context.RefreshToken
                .Include(x=>x.user)
                .FirstOrDefaultAsync(x => x.token == refreshToken);

            if (token == null || token.ExpiresIn < DateTime.UtcNow) return Unauthorized("the refresh token is expired");

            string acceessToken = _tokenService.GetAccessToken(token.user);

            token.token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            token.ExpiresIn = DateTime.UtcNow.AddDays(10);
            await _context.SaveChangesAsync();

            return Ok(new LogInResponse { JwtToken = acceessToken, refreshToken = token.token, Username = token.user.UserName });
            
        }
        private async Task<bool> checkUserExist(string email)
        {
            return await _context.users.AnyAsync(X=>X.EmailAddress==email.ToLower()&&X.isActive);
        }
    }
}