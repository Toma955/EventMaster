using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetEventTypes()
        {
            var eventTypes = await _context.EventTypes.ToListAsync();
            return Ok(eventTypes);
        }

        [HttpGet("performers/types")]
        public async Task<IActionResult> GetPerformerTypes()
        {
            var performerTypes = await _context.PerformerTypes.ToListAsync();
            return Ok(performerTypes);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Admin)
                .Include(e => e.EventPerformers)
                    .ThenInclude(ep => ep.Performer)
                        .ThenInclude(p => p.PerformerType)
                .ToListAsync();
            
            return Ok(events);
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Admin)
                .Include(e => e.EventPerformers)
                    .ThenInclude(ep => ep.Performer)
                        .ThenInclude(p => p.PerformerType)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (eventItem == null)
                return NotFound();
            
            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event eventItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }

        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event eventItem)
        {
            if (id != eventItem.Id)
                return BadRequest();

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
                return NotFound();

            existingEvent.Naziv = eventItem.Naziv;
            existingEvent.Opis = eventItem.Opis;
            existingEvent.Datum = eventItem.Datum;
            existingEvent.Lokacija = eventItem.Lokacija;
            existingEvent.EventTypeId = eventItem.EventTypeId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/events/performers
        [HttpGet("performers")]
        public async Task<IActionResult> GetPerformers()
        {
            var performers = await _context.Performers
                .Include(p => p.PerformerType)
                .ToListAsync();
            
            return Ok(performers);
        }

        // POST: api/events/performers
        [HttpPost("performers")]
        public async Task<IActionResult> CreatePerformer([FromBody] Performer performer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Performers.Add(performer);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetPerformers), new { id = performer.Id }, performer);
        }

        // PUT: api/events/performers/{id}
        [HttpPut("performers/{id}")]
        public async Task<IActionResult> UpdatePerformer(int id, [FromBody] Performer performer)
        {
            if (id != performer.Id)
                return BadRequest();

            var existingPerformer = await _context.Performers.FindAsync(id);
            if (existingPerformer == null)
                return NotFound();

            existingPerformer.Ime = performer.Ime;
            existingPerformer.Opis = performer.Opis;
            existingPerformer.PerformerTypeId = performer.PerformerTypeId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/events/performers/{id}
        [HttpDelete("performers/{id}")]
        public async Task<IActionResult> DeletePerformer(int id)
        {
            var performer = await _context.Performers.FindAsync(id);
            if (performer == null)
                return NotFound();

            _context.Performers.Remove(performer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/events/{eventId}/performers/{performerId}
        [HttpPost("{eventId}/performers/{performerId}")]
        public async Task<IActionResult> AssignPerformerToEvent(int eventId, int performerId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            var performer = await _context.Performers.FindAsync(performerId);
            
            if (eventItem == null || performer == null)
                return NotFound();

            var eventPerformer = new EventPerformer
            {
                EventId = eventId,
                PerformerId = performerId
            };

            _context.EventPerformers.Add(eventPerformer);
            await _context.SaveChangesAsync();
            
            return Ok("Performer successfully assigned to event");
        }

        // DELETE: api/events/{eventId}/performers/{performerId}
        [HttpDelete("{eventId}/performers/{performerId}")]
        public async Task<IActionResult> RemovePerformerFromEvent(int eventId, int performerId)
        {
            var eventPerformer = await _context.EventPerformers
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.PerformerId == performerId);
            
            if (eventPerformer == null)
                return NotFound();

            _context.EventPerformers.Remove(eventPerformer);
            await _context.SaveChangesAsync();
            
            return Ok("Performer successfully removed from event");
        }

        // POST: api/events/{id}/subscribe
        [HttpPost("{eventId}/subscribe")]
        public async Task<IActionResult> SubscribeToEvent(int eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
                return NotFound("Event not found");

            var subscription = new Subscription
            {
                EventId = eventId,
                UserId = 1, // TODO: Get from current user
                SubscribedAt = DateTime.Now
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            
            return Ok("Successfully subscribed to event");
        }

        // DELETE: api/events/{id}/unsubscribe
        [HttpDelete("{eventId}/unsubscribe")]
        public async Task<IActionResult> UnsubscribeFromEvent(int eventId)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.EventId == eventId && s.UserId == 1); // TODO: Get from current user
            
            if (subscription == null)
                return NotFound("Subscription not found");

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            
            return Ok("Successfully unsubscribed from event");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("protected-test")]
        public IActionResult ProtectedTest()
        {
            return Ok($"Ovo je zaštićen endpoint! Prijavljeni korisnik: {User.Identity?.Name}");
        }
    }
} 