using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Domain.DTOs;
using challenge.Infrastructure.Services;

namespace challenge.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly ChallengeContext _context;
        private readonly IHateoasService _hateoasService;

        public VehiclesController(ChallengeContext context, IHateoasService hateoasService)
        {
            _context = context;
            _hateoasService = hateoasService;
        }

        /// <summary>
        /// Get all vehicles with pagination
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
        /// <response code="200">Return paginated vehicles</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetVehicles")]
        public async Task<ActionResult<PagedResult<Vehicle>>> GetVehicles(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var pagingParams = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            
            var totalItems = await _context.Vehicles.CountAsync();
            var vehicles = await _context.Vehicles
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            var result = new PagedResult<Vehicle>
            {
                Items = vehicles,
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalItems = totalItems
            };

            // Adicionar links HATEOAS
            result.Links = _hateoasService.GeneratePaginationLinks(result, "Vehicle", Url);

            return Ok(result);
        }

        /// <summary>
        /// Get a specific vehicle by ID
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <response code="200">Return the vehicle</response>
        /// <response code="404">Vehicle not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetVehicle")]
        public async Task<ActionResult<Vehicle>> GetVehicle(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound(new { error = "Veículo não encontrado", vehicleId = id });
            }

            return Ok(vehicle);
        }

        /// <summary>
        /// Update an existing vehicle
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <param name="vehicle">Dados do veículo para atualização</param>
        /// <response code="204">Vehicle updated successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">Vehicle not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}", Name = "UpdateVehicle")]
        public async Task<IActionResult> PutVehicle(Guid id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    error = "Modelo inválido",
                    details = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            if (id != vehicle.VehicleId)
            {
                return BadRequest(new { error = "O ID na URL não corresponde ao ID do veículo no corpo da requisição." });
            }

            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound($"Veículo com ID {id} não encontrado.");
            }

            try
            {
                existingVehicle.LicensePlate = vehicle.LicensePlate;
                existingVehicle.VehicleModel = vehicle.VehicleModel;
                existingVehicle.IsCancel = vehicle.IsCancel;
                existingVehicle.UserCancelID = vehicle.UserCancelID;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound($"Veículo com ID {id} não encontrado durante atualização.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log do erro com detalhes para debug
                Console.WriteLine($"Erro ao atualizar veículo: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // Retornar erro com detalhes para facilitar diagnóstico
                return StatusCode(500, new { error = "Erro interno", message = ex.Message, innerException = ex.InnerException?.Message });
            }

            return NoContent();
        }
        /// <summary>
        /// Create a new vehicle
        /// </summary>
        /// <param name="vehicle">Dados do veículo para criação</param>
        /// <response code="201">Vehicle created successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreateVehicle")]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleId }, vehicle);
        }

        /// <summary>
        /// Delete a vehicle
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <response code="204">Vehicle successfully removed</response>
        /// <response code="404">Vehicle not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}", Name = "DeleteVehicle")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(Guid id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
