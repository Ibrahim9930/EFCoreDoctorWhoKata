using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using DoctorWho.Db.Utilities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DoctorWho.Tests
{
    public class DoctorWhoTests
    {
        private readonly DoctorWhoCoreDbContext _queryContext;
        private readonly DoctorWhoCoreDbContext _entitiesContext;
        private readonly DoctorWhoCoreDbContext _disconnectedContext;
        
        public DoctorWhoTests()
        {
            var optBuilderForSeededDb = new DbContextOptionsBuilder();
            optBuilderForSeededDb.UseInMemoryDatabase("DoctorWhoTestsDB");

            var optBuilderForUnseededDb = new DbContextOptionsBuilder();
            
            _queryContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);
            _entitiesContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);
            _disconnectedContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);

            _queryContext.Database.EnsureCreated();
        }


        [Fact]
        public void AddEnemy_ShouldAddEnemyInEnemyList()
        {
            Episode episode = _queryContext.Episodes.Find(1);
            Enemy enemy = _queryContext.Enemies.Find(1);

            episode.Context = _queryContext;
            enemy.Context = _queryContext;


            episode.AddEnemy(enemy);


            episode = _queryContext.Episodes
                .Include(ep => ep.Enemies).First(ep => ep.EpisodeId == 1);

            episode.Enemies.Should().Contain(en => en.EnemyId == enemy.EnemyId);
        }

        [Fact]
        public void AddEnemy_ShouldNotUpdateEpisodeWhenEnemyAddedWhenUntracked()
        {
            Episode episode = _queryContext.Episodes.Find(1);
            Enemy enemy = _queryContext.Enemies.Find(1);

            episode.Context = _entitiesContext;
            enemy.Context = _entitiesContext;

            string newTitle = "new title";
            episode.Title = newTitle;

            episode.AddEnemy(enemy);

            episode = _disconnectedContext.Episodes.Find(1);

            episode.Title.Should().NotBe(newTitle);
        }

        [Fact]
        public void AddCompanion_ShouldAddCompanionInCompanionList()
        {
            Episode episode = _queryContext.Episodes.Find(1);
            Companion companion = _queryContext.Companions.Find(1);

            episode.Context = _queryContext;
            companion.Context = _queryContext;


            episode.AddCompanion(companion);


            episode = _queryContext.Episodes
                .Include(ep => ep.Companions).First(ep => ep.EpisodeId == 1);

            episode.Companions.Should().Contain(c => c.CompanionId == companion.CompanionId);
        }

        [Fact]
        public void AddCompanion_ShouldNotUpdateEpisodeWhenCompanionAddedWhenUntracked()
        {
            Episode episode = _queryContext.Episodes.Find(1);
            Companion companion = _queryContext.Companions.Find(1);

            episode.Context = _entitiesContext;
            companion.Context = _entitiesContext;

            string newTitle = "new title";
            episode.Title = newTitle;

            episode.AddCompanion(companion);

            episode = _disconnectedContext.Episodes.Find(1);

            episode.Title.Should().NotBe(newTitle);
        }

        [Fact]
        public void GetEnemyById_EnemyExists_ShouldReturnEnemyWithRightNameAndId()
        {
            string enemyFakesPath = FakeDataGenerator.GetFilePathForTypeFakes<Enemy>();
            string listJson = File.ReadAllText(enemyFakesPath);

            List<Enemy> enemies = JsonSerializer.Deserialize<List<Enemy>>(listJson);
            Enemy targetEnemy = enemies.First(e => e.EnemyId == 1);

            Enemy enemy = Enemy.GetEnemyById(1, _queryContext);

            using (var scope = new AssertionScope())
            {
                enemy.EnemyId.Should().Be(targetEnemy.EnemyId);
                enemy.EnemyName.Should().Be(targetEnemy.EnemyName);
            }
        }

        [Fact]
        public void GetEnemyById_EnemyDoesntExist_ShouldReturnNull()
        {
            Enemy enemy = Enemy.GetEnemyById(11, _queryContext);

            enemy.Should().BeNull();
        }
        [Fact]
        public void GetCompanionById_CompanionExists_ShouldReturnCompanionWithRightNameAndId()
        {
            string companionFakesPath = FakeDataGenerator.GetFilePathForTypeFakes<Companion>();
            string listJson = File.ReadAllText(companionFakesPath);

            List<Companion> companions = JsonSerializer.Deserialize<List<Companion>>(listJson);
            Companion targetCompanion = companions.First(e => e.CompanionId == 1);

            Companion companion = Companion.GetCompanionById(1, _queryContext);

            using (var scope = new AssertionScope())
            {
                companion.CompanionName.Should().Be(targetCompanion.CompanionName);
                companion.CompanionId.Should().Be(targetCompanion.CompanionId);
            }
        }

        [Fact]
        public void GetCompanionById_CompanionDoesntExist_ShouldReturnNull()
        {
            Companion companion = Companion.GetCompanionById(11, _queryContext);

            companion.Should().BeNull();
        }
        [Fact]
        public void GetAllDoctors_StoreHasDoctors_ShouldReturnAllDoctorsWithTheRightNamesAndIds()
        {
            string doctorFakesPath = FakeDataGenerator.GetFilePathForTypeFakes<Doctor>();
            string listJson = File.ReadAllText(doctorFakesPath);

            List<Doctor> targetDoctors = JsonSerializer.Deserialize<List<Doctor>>(listJson);

            List<Doctor> doctors = Doctor.GetAllDoctors(_queryContext).ToList();

            doctors.Should().OnlyContain(
                doc => targetDoctors
                    .First(
                        td => td.DoctorId == doc.DoctorId && td.DoctorName == doc.DoctorName
                        ) != null
                );
        }
        
        [Fact]
        public void GetAllDoctors_StoreDoesntHaveDoctors_ShouldReturnNull()
        {
            // _queryContext.Doctors.RemoveRange(_queryContext.Doctors);
            //
            // List<Doctor> doctors = Doctor.GetAllDoctors(_queryContext).ToList();
            //
            // doctors.Should().BeNull();
        }
    }
}