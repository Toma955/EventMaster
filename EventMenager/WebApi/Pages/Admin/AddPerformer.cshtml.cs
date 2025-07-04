using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Pages.Admin
{
    public class AddPerformerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddPerformerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Performer Performer { get; set; } = new();

        public List<PerformerType> PerformerTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            PerformerTypes = await _context.PerformerTypes.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PerformerTypes = await _context.PerformerTypes.ToListAsync();
                return Page();
            }

            _context.Performers.Add(Performer);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Izvođač uspješno dodan!";
            return RedirectToPage("/Admin/Index");
        }
    }
} 