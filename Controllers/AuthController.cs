using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace challenge.Controllers
{
    /// <summary>
    /// Controller para autenticação e geração de tokens JWT
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gera um token JWT para autenticação
        /// </summary>
        /// <param name="request">Credenciais de login</param>
        /// <response code="200">Token gerado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validação simples (em produção, validar contra banco de dados)
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Username e password são obrigatórios" });
            }

            // Validação de credenciais (exemplo simplificado)
            // Em produção, validar contra banco de dados com hash de senha
            if (request.Username == "admin" && request.Password == "admin123" ||
                request.Username == "user" && request.Password == "user123")
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new
                {
                    token = token,
                    type = "Bearer",
                    expiresIn = 3600,
                    username = request.Username
                });
            }

            return Unauthorized(new { error = "Credenciais inválidas" });
        }

        /// <summary>
        /// Valida se o token JWT está válido
        /// </summary>
        /// <response code="200">Token válido</response>
        /// <response code="401">Token inválido ou expirado</response>
        [HttpGet("validate")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult ValidateToken()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new
            {
                valid = true,
                username = username,
                message = "Token válido"
            });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "ChaveSecretaSuperSeguraParaDesenvolvimento123456";
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, username == "admin" ? "Admin" : "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    /// <summary>
    /// Modelo de requisição de login
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
