using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<PerformerType> PerformerTypes { get; set; }
        public DbSet<EventPerformer> EventPerformers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventPerformer>()
                .HasKey(ep => new { ep.EventId, ep.PerformerId });

            modelBuilder.Entity<EventPerformer>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPerformers)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<EventPerformer>()
                .HasOne(ep => ep.Performer)
                .WithMany(p => p.EventPerformers)
                .HasForeignKey(ep => ep.PerformerId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Admin)
                .WithMany()
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeId);

            modelBuilder.Entity<Performer>()
                .HasOne(p => p.PerformerType)
                .WithMany(pt => pt.Performers)
                .HasForeignKey(p => p.PerformerTypeId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Event)
                .WithMany(e => e.Subscriptions)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasIndex(s => new { s.UserId, s.EventId })
                .IsUnique();
        }
    }
} 