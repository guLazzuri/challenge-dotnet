using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Domain.DTOs;
using challenge.Infrastructure.Services;

namespace challenge.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MaintenanceHistoriesController : ControllerBase
    {
        private readonly ChallengeContext _context;
        private readonly IHateoasService _hateoasService;

        public MaintenanceHistoriesController(ChallengeContext context, IHateoasService hateoasService)
        {
            _context = context;
            _hateoasService = hateoasService;
        }

        /// <summary>
        /// Get all maintenance histories with pagination
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
        /// <response code="200">Return paginated maintenance histories</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetMaintenanceHistories")]
        public async Task<ActionResult<PagedResult<MaintenanceHistory>>> GetMaintenanceHistories(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var pagingParams = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            
            var totalItems = await _context.MaintenanceHistories.CountAsync();
            var maintenanceHistories = await _context.MaintenanceHistories
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            var result = new PagedResult<MaintenanceHistory>
            {
                Items = maintenanceHistories,
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalItems = totalItems
            };

            // Adicionar links HATEOAS
            result.Links = _hateoasService.GeneratePaginationLinks(result, "MaintenanceHistory", Url);

            return Ok(result);
        }

        /// <summary>
        /// Get a specific maintenance history by ID
        /// </summary>
        /// <param name="id">ID do histórico de manutenção</param>
        /// <response code="200">Return the maintenance history</response>
        /// <response code="404">Maintenance history not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetMaintenanceHistory")]
        public async Task<ActionResult<MaintenanceHistory>> GetMaintenanceHistory(Guid id)
        {
            var maintenanceHistory = await _context.MaintenanceHistories.FindAsync(id);

            if (maintenanceHistory == null)
            {
                return NotFound(new { error = "Histórico de manutenção não encontrado", maintenanceHistoryId = id });
            }

            return Ok(maintenanceHistory);
        }

        /// <summary>
        /// Update an existing maintenance history
        /// </summary>
        /// <param name="id">ID do histórico de manutenção</param>
        /// <param name="maintenanceHistory">Dados do histórico para atualização</param>
        /// <response code="204">Maintenance history updated successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">Maintenance history not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}", Name = "UpdateMaintenanceHistory")]
        public async Task<IActionResult> PutMaintenanceHistory(Guid id, MaintenanceHistory maintenanceHistory)
        {
            if (id != maintenanceHistory.MaintenanceHistoryID)
            {
                return BadRequest();
            }

            _context.Entry(maintenanceHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new maintenance history
        /// </summary>
        /// <param name="maintenanceHistory">Dados do histórico para criação</param>
        /// <response code="201">Maintenance history created successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreateMaintenanceHistory")]
        public async Task<ActionResult<MaintenanceHistory>> PostMaintenanceHistory(MaintenanceHistory maintenanceHistory)
        {
            _context.MaintenanceHistories.Add(maintenanceHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaintenanceHistory", new { id = maintenanceHistory.MaintenanceHistoryID }, maintenanceHistory);
        }

        /// <summary>
        /// Delete a maintenance history
        /// </summary>
        /// <param name="id">ID do histórico de manutenção</param>
        /// <response code="204">Maintenance history successfully removed</response>
        /// <response code="404">Maintenance history not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}", Name = "DeleteMaintenanceHistory")]
        public async Task<IActionResult> DeleteMaintenanceHistory(Guid id)
        {
            var maintenanceHistory = await _context.MaintenanceHistories.FindAsync(id);
            if (maintenanceHistory == null)
            {
                return NotFound();
            }

            _context.MaintenanceHistories.Remove(maintenanceHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaintenanceHistoryExists(Guid id)
        {
            return _context.MaintenanceHistories.Any(e => e.MaintenanceHistoryID == id);
        }
    }
}
