using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HokkaidoBackend.Data;
using HokkaidoBackend.Models;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace HokkaidoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // API REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            if (_context.Users.Any(u => u.Email == userRegister.Email || u.PhoneNumber == userRegister.PhoneNumber))
            {
                return BadRequest(new { status = 400, message = "Email or Phone Number already in use" });
            }

            var newUser = new User
            {
                Name = userRegister.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password), // Hash the password using BCrypt
                PhoneNumber = userRegister.PhoneNumber,
                Email = userRegister.Email,
                Address = userRegister.Address,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { status = 200, message = "User registered successfully" });
        }

        // API LOGIN 
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == userLogin.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            {
                var token = GenerateJwtToken(user.Email);
                return Ok(new { status = 200, message = "success", data = token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // API GET USERS LIST
        //[HttpGet("users")]
        //public IActionResult GetUsers(int page = 1, int pageSize = 10)
        //{
        //    var users = _context.Users
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    var totalUsers = _context.Users.Count();

        //    return Ok(new
        //    {
        //        status = 200,
        //        message = "success",
        //        data = users,
        //        totalUsers = totalUsers,
        //        page = page,
        //        pageSize = pageSize
        //    });
        //}
    }
}
