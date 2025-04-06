using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.DtoModels;

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

            var eventItem = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                Date = eventDto.Date.ToUniversalTime(),
                CreatedBy = eventDto.CreatedBy,
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            // Добавляем участников через UserEvent
            if (eventDto.ParticipantsIds != null && eventDto.ParticipantsIds.Any())
            {
                foreach (var userId in eventDto.ParticipantsIds)
                {
                    var userEvent = new UserEvent
                    {
                        UserId = userId,
                        EventId = eventItem.Id
                    };
                    _context.UserEvents.Add(userEvent);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetEvents), new { id = eventItem.Id }, eventItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.UserEvents) 
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.UserEvents.Clear();
            await _context.SaveChangesAsync();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto eventDto)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound("Мероприятие не найдено.");
            }

            // Обновляем основные данные мероприятия
            eventItem.Title = eventDto.Title;
            eventItem.Description = eventDto.Description;
            eventItem.Date = eventDto.Date.ToUniversalTime();

            // Обновляем участников через таблицу UserEvents
            var existingParticipants = _context.UserEvents.Where(ue => ue.EventId == id).ToList();

            // Удаление участников, которые не были переданы в запросе
            var participantsToRemove = existingParticipants.Where(ue => !eventDto.ParticipantsIds.Contains(ue.UserId)).ToList();
            _context.UserEvents.RemoveRange(participantsToRemove);

            // Добавление новых участников
            var newParticipantIds = eventDto.ParticipantsIds.Except(existingParticipants.Select(ue => ue.UserId)).ToList();
            foreach (var userId in newParticipantIds)
            {
                _context.UserEvents.Add(new UserEvent { EventId = id, UserId = userId });
            }

            await _context.SaveChangesAsync();

            return Ok(eventItem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventItem = await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    ParticipantsIds = e.UserEvents.Select(ue => ue.UserId).ToList()
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound("Мероприятие не найдено.");
            }

            return Ok(eventItem);
        }
    }
}

