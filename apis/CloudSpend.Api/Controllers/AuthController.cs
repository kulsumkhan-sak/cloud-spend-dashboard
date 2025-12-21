using Microsoft.AspNetCore.Mvc;
using CloudSpend.Api.Models;
using CloudSpend.Api.Repos;
using Microsoft.AspNetCore.Identity;

namespace CloudSpend.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<string> _passwordHasher = new();

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("/api/auth/login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var passwordHash = _userRepository.GetPasswordHashByEmail(request.Email);

            if (passwordHash == null)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(
                request.Email,
                passwordHash,
                request.Password
            );

            if (verificationResult == PasswordVerificationResult.Success)
            {
                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = "Login successful"
                });
            }

            return Unauthorized(new LoginResponse
            {
                Success = false,
                Message = "Invalid email or password"
            });
        }
    }
}