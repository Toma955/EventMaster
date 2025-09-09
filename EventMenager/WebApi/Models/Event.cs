using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;
        
        public string? Opis { get; set; }
        
        [Required]
        public DateTime Datum { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Lokacija { get; set; } = string.Empty;
        
        public int EventTypeId { get; set; }
        public int AdminId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public virtual EventType EventType { get; set; } = null!;
        public virtual User Admin { get; set; } = null!;
        public virtual ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
} 