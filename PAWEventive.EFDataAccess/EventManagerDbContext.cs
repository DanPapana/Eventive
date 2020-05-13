using PAWEventive.ApplicationLogic.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAWEventive.EFDataAccess
{
    public class EventManagerDbContext : DbContext
    {
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<EventDetails> EventDetails { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDetails>().Property(P => P.ParticipationFee).HasColumnType("decimal(18,4)");
        }
    }
}
