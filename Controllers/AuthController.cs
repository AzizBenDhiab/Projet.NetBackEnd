using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetNET.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(User request)
        {
            // Check if the request object is null
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            // Validate required fields
            if (string.IsNullOrEmpty(request.LastName) ||
                string.IsNullOrEmpty(request.FirstName) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.PasswordHash) ||
                string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Adress) ||
                (request.IsAdmin == null))
            {
                return BadRequest("LastName, FirstName, Email, PasswordHash, Phone Number, Adress, and IsAdmin are required fields");
            }

            // Validate email format
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email format");
            }
            if (_db.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Email already exists");
            }

            // Validate password length or other criteria if needed
            if (request.PasswordHash.Length < 6)
            {
                return BadRequest("Password should be at least 6 characters long");
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            // Create a new user instance
            var user = new User
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PhoneNumber = request.PhoneNumber,
                Adress = request.Adress,
                IsAdmin = request.IsAdmin,
            };

            // Save the user to the database
            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok("User added successfully");
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginRequest loginRequest)
        {
            // Validate login request
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            // Find the user by email
            var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

            // Check if the user exists and verify the password
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate JWT token
            var token = GenerateTokenString(user);

            // Set the token in a cookie
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddHours(1), 
            });

            return Ok("Login successful");
        }

        // Validate email format
        [NonAction]
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        [NonAction]
        public string GenerateTokenString(User user)
        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
   
    

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
