using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsAPI.Models;
using NewsAPI.Services.Models;
using NewsAPI.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NewsAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static LoginDto user = new LoginDto();
        private readonly IUserRepository _service;
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration,IUserRepository servise)
        {
            _configuration = configuration;
            _service = servise;
        }
        // ---------------------------Get All Users EndPoint--------------------------------
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _service.GetAllUsers();
    
            return Ok(users);

        }
        // ---------------------------Get Users NameById EndPoint--------------------------------
        [HttpGet("{id}", Name = "GetUser"), Authorize]
        public IActionResult GetAuthor(int id)
        {
            var user = _service.GetUserName(id);
            if (user is null)
            {
                return NotFound();
            }
        
            return Ok(user);
        }
        // ---------------------------Get User Registration EndPoint--------------------------------
        [HttpPost("register")]
        public ActionResult<CreatedUser> createUser(UserDto user)
        {
            if (!_service.CheckUserExsit(user.Email))
            {
                var newUser = new User { Name = user.Name, Email = user.Email };
                var encripted = EncriptingPassword(user.Password);
                newUser.PasswordSalt = encripted.PasswordSalt;
                newUser.PasswordHash = encripted.PasswordHash;

                var createdUser = _service.AddUser(newUser);
                var client = new CreatedUser()
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    Name = createdUser.Name,
                };
                return Ok(client);
            }
            return BadRequest("User Already Exists");

        }
        // ---------------------------Get Users Login EndPoint--------------------------------
        [HttpPost("login")]
        public ActionResult<User> loginUser(LoginDto data)
        {
           var check = _service.CheckUserExsit(data.Email);

            if(check)
            {
                if (_service.VerifyFormPassword(data))
                {
                    
                    string token = CreateToken(data);
                    var dto = new PasswordDto();
                    dto = EncriptingPassword(data.Password);
                    var user = new User()
                    {
                       Email= data.Email,
                       PasswordHash = dto.PasswordHash,
                       PasswordSalt= dto.PasswordSalt,
                       AcessToken=token,
                    };
                    _service.SetAcessToken(user);
                    return Ok(token);
                }
                return BadRequest("Password InValid");
            }
           

            return BadRequest("User Not Found.");

        }
        // ---------------------------Password Encripting Process--------------------------------
        private PasswordDto EncriptingPassword(string password)
        {
            var encripted = new PasswordDto();
            using (HMACSHA512? hmac = new HMACSHA512())
            {
                encripted.PasswordSalt = hmac.Key;
                encripted.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return encripted;

        }
        // ---------------------------Create Token Process--------------------------------
        private string CreateToken(LoginDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
