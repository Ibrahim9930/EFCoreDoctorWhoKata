using System.Collections.Generic;
using System.Linq;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using DoctorWho.Db.Repositories;
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
        private readonly IEntityReader _entityReader;

        public DoctorWhoTests()
        {
            var optBuilderForSeededDb = new DbContextOptionsBuilder();
            optBuilderForSeededDb.UseInMemoryDatabase("DoctorWhoTestsDB");

            var optBuilderForUnseededDb = new DbContextOptionsBuilder();

            _queryContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);
            _entitiesContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);
            _disconnectedContext = new DoctorWhoCoreDbContext(optBuilderForSeededDb.Options);

            _queryContext.Database.EnsureCreated();

            _entityReader = new FakeDataReaderWriter();
        }


        [Fact]
        public void AddEnemy_ShouldAddEnemyInEnemyList()
        {
            EpisodeEFRepository episodeRepo = new EpisodeEFRepository(_queryContext);

            Episode episode = _queryContext.Episodes.Find(1);
            Enemy enemy = _queryContext.Enemies.Find(1);


            episodeRepo.AddEnemy(episode, enemy);
            episodeRepo.Commit();

            episode = _queryContext.Episodes
                .Include(ep => ep.Enemies).First(ep => ep.EpisodeId == 1);

            episode.Enemies.Should().Contain(en => en.EnemyId == enemy.EnemyId);
        }

        [Fact]
        public void AddEnemy_ShouldNotUpdateEpisodeWhenEnemyAddedWhenUntracked()
        {
            EpisodeEFRepository episodeRepo = new EpisodeEFRepository(_entitiesContext);

            Episode episode = _queryContext.Episodes.Find(1);
            Enemy enemy = _queryContext.Enemies.Find(1);


            string newTitle = "new title";
            episode.Title = newTitle;

            episodeRepo.AddEnemy(episode, enemy);
            episodeRepo.Commit();

            episode = _disconnectedContext.Episodes.Find(1);

            episode.Title.Should().NotBe(newTitle);
        }

        [Fact]
        public void AddCompanion_ShouldAddCompanionInCompanionList()
        {
            EpisodeEFRepository episodeRepo = new EpisodeEFRepository(_queryContext);

            Episode episode = _queryContext.Episodes.Find(1);
            Companion companion = _queryContext.Companions.Find(1);
            
            episodeRepo.AddCompanion(episode, companion);
            episodeRepo.Commit();


            episode = _queryContext.Episodes
                .Include(ep => ep.Companions).First(ep => ep.EpisodeId == 1);

            episode.Companions.Should().Contain(c => c.CompanionId == companion.CompanionId);
        }

        [Fact]
        public void AddCompanion_ShouldNotUpdateEpisodeWhenCompanionAddedWhenUntracked()
        {
            EpisodeEFRepository episodeRepo = new EpisodeEFRepository(_entitiesContext);

            Episode episode = _queryContext.Episodes.Find(1);
            Companion companion = _queryContext.Companions.Find(1);
            

            string newTitle = "new title";
            episode.Title = newTitle;

            episodeRepo.AddCompanion(episode,companion);
            episodeRepo.Commit();
            
            episode = _disconnectedContext.Episodes.Find(1);

            episode.Title.Should().NotBe(newTitle);
        }

        [Fact]
        public void EnemyEFRepositoryGetById_EnemyExists_ShouldReturnEnemyWithRightNameAndId()
        {
            EnemyEFRepository enemyRepo = new EnemyEFRepository(_queryContext);
            
            Enemy targetEnemy = _entityReader.ReadFakeEntity<Enemy>(1, e => e.EnemyId);

            Enemy enemy = enemyRepo.GetById(1);

            using (var scope = new AssertionScope())
            {
                enemy.EnemyId.Should().Be(targetEnemy.EnemyId);
                enemy.EnemyName.Should().Be(targetEnemy.EnemyName);
            }
        }

        [Fact]
        public void EnemyEFRepositoryGetById_EnemyDoesntExist_ShouldReturnNull()
        {
            EnemyEFRepository enemyRepo = new EnemyEFRepository(_queryContext);
            
            Enemy enemy = enemyRepo.GetById(11);

            enemy.Should().BeNull();
        }
        
        [Fact]
        public void EnemyEFRepositoryGetById_StoreHasDoctors_ShouldReturnAllDoctorsWithTheRightNamesAndIds()
        {
            EnemyEFRepository enemyRepo = new EnemyEFRepository(_queryContext);
            List<Enemy> targetDoctors = _entityReader.ReadAllFakeEntities<Enemy>().ToList();

            List<Enemy> doctors = enemyRepo.GetAllEntities().ToList();

            doctors.Should().OnlyContain(
                en => targetDoctors
                    .First(
                        te => te.EnemyId == en.EnemyId && te.EnemyName == en.EnemyName
                    ) != null
            );
        }
    }
}