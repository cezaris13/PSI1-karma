using System;
using Microsoft.EntityFrameworkCore;

namespace Karma.Models
{
    public class KarmaContext : DbContext
    {
        public DbSet<CharityEvent> Events { get; set; }

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<SpecialEquipment> SpecialEquipment { get; set; }

        public DbSet<EventImages> EventImages { get; set; }

        public DbSet<PendingVolunteer> PendingVolunteers { get; set; }

        public string DbPath { get; private set; }

        public KarmaContext(DbContextOptions options) : base(options)
        {

        }
    }
}
