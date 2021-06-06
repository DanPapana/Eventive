using Eventive.ApplicationLogic.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Eventive.EFDataAccess
{
    public class EventManagerDbContext : DbContext
    {
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }
        public DbSet<EventOrganized> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventApplication> Applications { get; set; }
        public DbSet<EventFollowing> Followings { get; set; }
        public DbSet<EventClick> Clicks { get; set; }
        public DbSet<EventDetails> EventDetails { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EventRating> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDetails>().Property(P => P.ParticipationFee).HasColumnType("decimal(18,2)");
        }
    }
}
