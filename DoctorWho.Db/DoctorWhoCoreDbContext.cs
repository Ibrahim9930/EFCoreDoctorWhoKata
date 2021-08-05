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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enemy>().HasMany(e => e.Episodes).WithMany(e => e.Enemies).UsingEntity(j =>
            {
                j.ToTable("EpisodeEnemy");
                j.Property("EpisodesEpisodeId").HasColumnName("EpisodeId");
                j.Property("EnemiesEnemyId").HasColumnName("EnemyId");
                j.Property<int>("EpisodeEnemyId");
                j.HasKey("EpisodeEnemyId");
            });

            modelBuilder.Entity<Companion>().HasMany(c => c.Episodes).WithMany(e => e.Companions).UsingEntity(j =>
            {
                j.ToTable("EpisodeCompanion");
                j.Property("EpisodesEpisodeId").HasColumnName("EpisodeId");
                j.Property("CompanionsCompanionId").HasColumnName("CompanionId");
                j.Property<int>("EpisodeCompanionId");
                j.HasKey("EpisodeCompanionId");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
