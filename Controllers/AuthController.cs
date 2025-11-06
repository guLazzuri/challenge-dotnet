
using challenge.Domain.Entity;
using challenge.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtService;

        public AuthController(JwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Aqui você faria validação de usuário no banco
            if (request.Username == "admin" && request.Password == "12345")
            {
                var token = _jwtService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }


}
