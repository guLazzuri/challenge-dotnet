using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace challenge.Controllers
{
    /// <summary>
    /// Controller para verificação de saúde da API
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Verifica o status de saúde da API e suas dependências
        /// </summary>
        /// <response code="200">API está saudável</response>
        /// <response code="503">API ou alguma dependência está com problemas</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var response = new
            {
                status = report.Status.ToString(),
                timestamp = DateTime.UtcNow,
                duration = report.TotalDuration,
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration,
                    exception = e.Value.Exception?.Message,
                    data = e.Value.Data
                }),
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(response)
                : StatusCode(503, response);
        }

        /// <summary>
        /// Endpoint simplificado para verificação rápida (usado por load balancers)
        /// </summary>
        /// <response code="200">API está pronta</response>
        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return Ok(new
            {
                status = "Ready",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Endpoint para verificar se a API está viva (liveness probe)
        /// </summary>
        /// <response code="200">API está viva</response>
        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                status = "Alive",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
