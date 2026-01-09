using Microsoft.AspNetCore.Mvc;
using CloudSpend.Api.Models;
using CloudSpend.Api.Repos;
using CloudSpend.Api.Managers;
using Microsoft.AspNetCore.Identity;
using Google.Apis.Auth;

namespace CloudSpend.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AuthManager _authManager;
        private readonly PasswordHasher<string> _passwordHasher = new();

        public AuthController(
            UserRepository userRepository,
            AuthManager authManager
        )
        {
            _userRepository = userRepository;
            _authManager = authManager;
        }

        // ============================
        // EMAIL + PASSWORD LOGIN
        // (NO JWT — SAME AS BEFORE)
        // ============================
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

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            // ✅ SAME OLD RESPONSE (no JWT)
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful"
            });
        }

        // ============================
        // GOOGLE LOGIN (JWT ONLY HERE)
        // ============================
        [HttpPost("/api/auth/google")]
        public async Task<IActionResult> GoogleLogin(
            [FromBody] CloudSpend.Api.Models.GoogleLoginRequest request
        )
        {
            if (string.IsNullOrWhiteSpace(request?.IdToken))
            {
                return BadRequest("Missing Google token");
            }

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            }
            catch
            {
                return Unauthorized("Invalid Google token");
            }

            var email = payload.Email;
            var name = payload.Name;
            var googleId = payload.Subject;

            // 🔐 JWT ONLY FOR GOOGLE
            var token = _authManager.GenerateJwt(email, name, googleId);

            return Ok(new
            {
                token,
                user = new
                {
                    email = email,
                    name = name,
                    picture = payload.Picture
                }
            });
        }
    }
}