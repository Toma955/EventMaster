namespace WebApi.Models
{
    public class EventPerformer
    {
        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;
        
        public int PerformerId { get; set; }
        public virtual Performer Performer { get; set; } = null!;
    }
} 