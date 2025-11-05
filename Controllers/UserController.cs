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
    public class UserController : ControllerBase
    {
        private readonly ChallengeContext _context;
        private readonly IHateoasService _hateoasService;

        public UserController(ChallengeContext context, IHateoasService hateoasService)
        {
            _context = context;
            _hateoasService = hateoasService;
        }

        /// <summary>
        /// Get all users with pagination
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
        /// <response code="200">Return paginated users</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<PagedResult<User>>> GetUsers(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var pagingParams = new PagingParameters { PageNumber = pageNumber, PageSize = pageSize };
            
            var totalItems = await _context.Users.CountAsync();
            var users = await _context.Users
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            var result = new PagedResult<User>
            {
                Items = users,
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalItems = totalItems
            };

            // Adicionar links HATEOAS
            result.Links = _hateoasService.GeneratePaginationLinks(result, "User", Url);

            return Ok(result);
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <response code="200">Return the user</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { error = "Usuário não encontrado", userId = id });
            }

            return Ok(user);
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="user">Dados do usuário para atualização</param>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// Create a new user
        /// </summary>
        /// <param name="user">Dados do usuário para criação</param>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreateUser")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <response code="204">User successfully removed</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
