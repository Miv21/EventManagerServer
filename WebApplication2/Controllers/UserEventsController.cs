using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{eventId}")]
        public async Task<IActionResult> AddUserToEvent(int eventId, [FromBody] User user)
        {
            var existingEvent = await _context.Events.Include(e => e.Participants)
                                                     .FirstOrDefaultAsync(e => e.Id == eventId);
            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            if (!existingEvent.Participants.Contains(existingUser))
            {
                existingEvent.Participants.Add(existingUser);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest("User already added to the event.");
        }

    }
}
