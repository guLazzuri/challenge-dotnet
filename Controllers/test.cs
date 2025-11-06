using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace challenge.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var username = User.Identity?.Name ?? "usuário desconhecido";
            return Ok(new { message = $"Olá, {username}! Você acessou um endpoint protegido com JWT." });
        }
    }
}