using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSolution.Data;
using InventoryManagementSolution.Models;
using InventoryManagementSolution.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagementSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsersController(MyDbContext context)
        {
            _context = context;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterUserDto registerUserDto)
        {
            if (await _context.appuser.AnyAsync(u => u.emailaddress == registerUserDto.EmailAddress))
            {
                return BadRequest("Email already in use.");
            }

            CreatePasswordHash(registerUserDto.Password, out string passwordHash, out string salt);

            var internalAuth = new InternalAuthentication
            {
                passwordhash = passwordHash,
                salt = salt
            };

            var appUser = new AppUser
            {
                firstname = registerUserDto.FirstName,
                lastname = registerUserDto.LastName,
                emailaddress = registerUserDto.EmailAddress,
                creation = DateTime.UtcNow,
                lastlogin = DateTime.UtcNow,
                InternalAuthentication = internalAuth
            };

            _context.appuser.Add(appUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppUser), new { id = appUser.id }, appUser);
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            Console.WriteLine(loginDto.EmailAddress);
            var user = await _context.appuser
                .Include(u => u.InternalAuthentication)
                .FirstOrDefaultAsync(u => u.emailaddress == loginDto.EmailAddress);
            
            
            if (user == null || user.InternalAuthentication == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!VerifyPasswordHash(loginDto.Password, user.InternalAuthentication.passwordhash, user.InternalAuthentication.salt))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Optionally generate and return a JWT token here

            return Ok("Login successful.");
        }

        // Utility method to hash a password
        private void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        // Utility method to verify a password
        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                Console.WriteLine(Convert.ToBase64String(computedHash));
                Console.WriteLine(storedHash);
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<AppUser>> GetAppUserByEmail(string email)
        {
            var appUser = await _context.appuser
                .Include(u => u.InternalAuthentication)
                .FirstOrDefaultAsync(u => u.emailaddress == email);

            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }
        
        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUser(int id)
        {
            var appUser = await _context.appuser
                .Include(u => u.InternalAuthentication)
                .FirstOrDefaultAsync(u => u.id == id);

            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }
    }
}
