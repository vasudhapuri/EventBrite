using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class EventContext: DbContext
    {
        public EventContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventLocation> EventLocations { get; set; }
        public DbSet<EachEvent> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventType>(e =>
            {
                e.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(100);

            });

            modelBuilder.Entity<EventLocation>(e =>
            {
                e.Property(l => l.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(l => l.City)
                .IsRequired()
                .HasMaxLength(100);

            });

            modelBuilder.Entity<EachEvent>(e =>
            {
                e.Property(v => v.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(v => v.EventName)
                .IsRequired()
                .HasMaxLength(100);

                e.Property(v => v.Price)
                .IsRequired().HasColumnType("decimal(5,2)");

                e.Property(v => v.Date)
                .IsRequired();

                e.HasOne(v => v.EventType)
                .WithMany()
                .HasForeignKey(v => v.TypeId);

                e.HasOne(v => v.EventLocation)
                .WithMany()
                .HasForeignKey(v => v.LocationId);

                e.Property(v => v.Address)
                .IsRequired();

                e.Property(v => v.State)
                .IsRequired()
                .HasMaxLength(10);



            });

        }
    

    }
}
