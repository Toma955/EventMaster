using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class PerformerType
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Naziv { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Opis { get; set; }
        
        // Navigation property
        public virtual ICollection<Performer> Performers { get; set; } = new List<Performer>();
    }
} 