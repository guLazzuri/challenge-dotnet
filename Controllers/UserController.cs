using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;

namespace challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ChallengeContext _context;

        public UserController(ChallengeContext context)
        {
            _context = context;
        }

        // GET: api/user
        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">Return all users</response>
        /// <response code="404">No User Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">Return all users</response>
        /// <response code="404">No User Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="204">User successfully updated</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">No User Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">No User Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="204">User successfully removed</response>
        /// <response code="404">No User Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
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
