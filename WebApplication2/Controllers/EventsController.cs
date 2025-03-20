using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получить все мероприятия с нужными данными
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id, 
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                })
                .ToListAsync();

            return events;
        }

        // Создать новое мероприятие
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest("Event item is required.");
            }

            // Получаем участников по их ID
            var participants = new List<User>();
            if (eventDto.ParticipantsIds != null && eventDto.ParticipantsIds.Any())
            {
                foreach (var participantId in eventDto.ParticipantsIds)
                {
                    var user = await _context.Users.FindAsync(participantId);
                    if (user != null)
                    {
                        participants.Add(user);
                    }
                }
            }

            var eventItem = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                Date = eventDto.Date.ToUniversalTime(),
                CreatedBy = eventDto.CreatedBy,
                Participants = participants
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = eventItem.Id }, eventItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Participants) 
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.Participants.Clear();
            await _context.SaveChangesAsync();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; } 
        public List<int> ParticipantsIds { get; set; }  
    }

}

