using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Performer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Ime { get; set; } = string.Empty;
        
        public string? Opis { get; set; }
        
        public int PerformerTypeId { get; set; }
        
        // Navigation properties
        public virtual PerformerType PerformerType { get; set; } = null!;
        public virtual ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();
    }
} 