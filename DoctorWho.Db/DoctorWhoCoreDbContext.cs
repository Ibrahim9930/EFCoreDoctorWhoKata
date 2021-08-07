using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using DoctorWho.Db.Domain;
using DoctorWho.Db.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DoctorWho.Db
{
    public class DoctorWhoCoreDbContext : DbContext
    {
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Companion> Companions { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<EpisodeDetails> EpisodeDetails { get; set; }

        public string GetCompanionNamesForEpisode(int episodeId)
        {
            throw new NotSupportedException();
        }

        public string GetEnemyNamesForEpisode(int episodeId)
        {
            throw new NotSupportedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            );
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GetJoinEntityBuilder<Enemy, Episode>(modelBuilder).UsingEntity(j =>
            {
                j.ToTable("EpisodeEnemy");
                j.Property("EpisodesEpisodeId").HasColumnName("EpisodeId");
                j.Property("EnemiesEnemyId").HasColumnName("EnemyId");
                j.Property<int>("EpisodeEnemyId");
                j.HasKey("EpisodeEnemyId");
            });

            GetJoinEntityBuilder<Companion, Episode>(modelBuilder).UsingEntity(j =>
            {
                j.ToTable("EpisodeCompanion");
                j.Property("EpisodesEpisodeId").HasColumnName("EpisodeId");
                j.Property("CompanionsCompanionId").HasColumnName("CompanionId");
                j.Property<int>("EpisodeCompanionId");
                j.HasKey("EpisodeCompanionId");
            });

            modelBuilder
                .HasDbFunction(typeof(DoctorWhoCoreDbContext).GetMethod(nameof(GetCompanionNamesForEpisode),
                    new[] {typeof(int)})).HasName("fnCompanions");
            modelBuilder
                .HasDbFunction(
                    typeof(DoctorWhoCoreDbContext).GetMethod(nameof(GetEnemyNamesForEpisode), new[] {typeof(int)}))
                .HasName("fnEnemies");

            modelBuilder.Entity<EpisodeDetails>().HasNoKey().ToView("viewEpisodes");
            modelBuilder.Entity<EpisodeDetails>().Property(ed => ed.CompanionsNames)
                .HasColumnName("Companions");
            modelBuilder.Entity<EpisodeDetails>().Property(ed => ed.EnemiesNames)
                .HasColumnName("Enemies");

            SeedModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedModel(ModelBuilder modelBuilder)
        {
            SeedEntities(modelBuilder);
            SeedJoinEntities(modelBuilder);
        }

        private void SeedEntities(ModelBuilder modelBuilder)
        {
            SeedEntity<Author>(modelBuilder);
            SeedEntity<Enemy>(modelBuilder);
            SeedEntity<Doctor>(modelBuilder);
            SeedEntity<Companion>(modelBuilder);
            SeedEntity<Episode>(modelBuilder);
        }

        private void SeedEntity<T>(ModelBuilder modelBuilder) where T : class
        {
            string fakeDataFilePath = FakeDataGenerator.GetFilePathForTypeFakes<T>();

            string listJson = File.ReadAllText(fakeDataFilePath);
            List<T> entities = JsonSerializer.Deserialize<List<T>>(listJson) ?? new List<T>();

            modelBuilder.Entity<T>().HasData(entities);
        }

        private void SeedJoinEntities(ModelBuilder modelBuilder)
        {
            string enemyEpisodeFakeDataFilePath = FakeDataGenerator.GetFilePathForJoinTypeFakes<Enemy, Episode>();

            string enemyEpisodeJsonList = File.ReadAllText(enemyEpisodeFakeDataFilePath);
            List<IDictionary<string, int>> enemyEpisodeEntities =
                JsonSerializer.Deserialize<List<IDictionary<string, int>>>(enemyEpisodeJsonList) ??
                new List<IDictionary<string, int>>();
            var mappedEnemyEpisodes
                = enemyEpisodeEntities.Select(e => new
                {
                    EpisodesEpisodeId = e["EpisodesEpisodeId"],
                    EnemiesEnemyId = e["EnemiesEnemyId"],
                    EpisodeEnemyId = e["EpisodeEnemyId"],
                });

            GetJoinEntityBuilder<Enemy, Episode>(modelBuilder)
                .UsingEntity(j => j.HasData(mappedEnemyEpisodes));


            string companionEpisodeFakeDataFilePath =
                FakeDataGenerator.GetFilePathForJoinTypeFakes<Companion, Episode>();

            string companionEpisodeJsonList = File.ReadAllText(companionEpisodeFakeDataFilePath);
            List<IDictionary<string, int>> companionEpisodeEntities =
                JsonSerializer.Deserialize<List<IDictionary<string, int>>>(companionEpisodeJsonList) ??
                new List<IDictionary<string, int>>();
            var mappedCompanionEpisodes
                = companionEpisodeEntities.Select(e => new
                {
                    EpisodesEpisodeId = e["EpisodesEpisodeId"],
                    CompanionsCompanionId = e["CompanionsCompanionId"],
                    EpisodeCompanionId = e["EpisodeCompanionId"]
                });

            GetJoinEntityBuilder<Companion, Episode>(modelBuilder)
                .UsingEntity(j => j.HasData(mappedCompanionEpisodes));
        }

        private static CollectionCollectionBuilder<TSecondEntityType, TFirstEntityType>
            GetJoinEntityBuilder<TFirstEntityType, TSecondEntityType>(ModelBuilder modelBuilder)
            where TFirstEntityType : class where TSecondEntityType : class
        {
            var firstEntityCollectionProperty = GetNavigationCollectionProperty<TFirstEntityType, TSecondEntityType>();
            var secondEntityCollectionProperty = GetNavigationCollectionProperty<TSecondEntityType, TFirstEntityType>();

            var entityBuilder = modelBuilder.Entity<TFirstEntityType>()
                .HasMany<TSecondEntityType>(
                    firstEntityCollectionProperty.Name
                )
                .WithMany(
                    secondEntityCollectionProperty.Name
                );

            return entityBuilder;
        }

        private static PropertyInfo GetNavigationCollectionProperty<TFirstEntityType, TSecondEntityType>()
            where TFirstEntityType : class where TSecondEntityType : class
        {
            var properties = typeof(TFirstEntityType).GetProperties();

            PropertyInfo collectionProperty = null;
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(ICollection<TSecondEntityType>))
                    collectionProperty = property;
            }

            return collectionProperty;
        }
    }
}