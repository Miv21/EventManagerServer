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
            var existingEvent = await _context.Events.FindAsync(eventId);
            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Проверяем, существует ли уже связь в таблице UserEvents
            var userEventExists = await _context.UserEvents
                .AnyAsync(ue => ue.EventId == eventId && ue.UserId == user.Id);

            if (userEventExists)
            {
                return BadRequest("User already added to the event.");
            }

            // Добавляем связь в UserEvents
            var userEvent = new UserEvent
            {
                EventId = eventId,
                UserId = user.Id
            };

            _context.UserEvents.Add(userEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}