using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class EventType
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Naziv { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Opis { get; set; }
        
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
} 