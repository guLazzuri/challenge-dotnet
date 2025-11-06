using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Domain.DTOs;
using challenge.Infrastructure.Services;
using challenge.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace challenge.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly ChallengeContext _context;
        private readonly IHateoasService _hateoasService;
        private readonly TokenService _tokenService;
        private readonly IRepository<User> _userRepository;
        private IHateoasService @object;

        [ActivatorUtilitiesConstructor]
        public UserController(ChallengeContext context, IHateoasService hateoasService, TokenService tokenService, IRepository<User> userRepository)
        {
            _context = context;
            _hateoasService = hateoasService;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public UserController(ChallengeContext context, IHateoasService @object)
        {
            _context = context;
            this.@object = @object;
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
        [Authorize(Roles = "ADMIN")] // Apenas ADMIN pode listar usuários
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
        [Authorize(Roles = "ADMIN")] // Apenas ADMIN pode buscar usuário por ID
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
        [Authorize(Roles = "ADMIN,CLIENT")] // ADMIN e CLIENT podem atualizar (assumindo que CLIENT só atualiza o próprio)
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
        [AllowAnonymous] // Permitir criação de usuário sem autenticação (registro)
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
        [Authorize(Roles = "ADMIN")] // Apenas ADMIN pode deletar usuário
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

        /// <summary>
        /// Authenticate a user and return a JWT token
        /// </summary>
        /// <param name="loginDto">Credenciais do usuário (Email e Password)</param>
        /// <response code="200">Return the JWT token</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login", Name = "Login")]
        [AllowAnonymous] // Permitir login sem autenticação
        public async Task<ActionResult<object>> Authenticate([FromBody] LoginDto loginDto)
        {
            // Nota: Em um cenário real, a senha seria hasheada e comparada com o hash armazenado.
            // Como não temos a implementação de hash, faremos uma comparação simples.
            // O ideal seria usar o ASP.NET Core Identity ou uma biblioteca de hashing como BCrypt.

            var user = await _userRepository.Get(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Gera o Token
            var token = _tokenService.GenerateToken(user);

            // Retorna o Token e informações básicas do usuário
            return Ok(new
            {
                user = new { user.UserID, user.Email, user.Type },
                token = token
            });
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
