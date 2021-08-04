using System;
using DoctorWho.Db.Domain;
using Microsoft.EntityFrameworkCore;

namespace DoctorWho.Db
{
    public class DoctorWhoCoreDbContext : DbContext
    {
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Companion> Companions { get; set; }
        public DbSet<Enemy> Enemies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(

            );
            base.OnConfiguring(optionsBuilder);
        }


    }
}
