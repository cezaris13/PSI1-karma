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

        public string DbPath { get; private set; }

        public KarmaContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}karma.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
