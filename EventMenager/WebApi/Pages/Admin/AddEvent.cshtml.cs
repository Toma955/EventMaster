using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Pages.Admin
{
    public class AddEventModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddEventModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; } = new();

        public List<EventType> EventTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            EventTypes = await _context.EventTypes.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                EventTypes = await _context.EventTypes.ToListAsync();
                return Page();
            }

            // Set admin ID (hardcoded for now, should come from authentication)
            Event.AdminId = 1;
            Event.CreatedAt = DateTime.Now;

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Događaj uspješno dodan!";
            return RedirectToPage("/Admin/Index");
        }
    }
} 