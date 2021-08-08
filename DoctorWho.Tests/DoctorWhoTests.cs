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
            var optBuilder = new DbContextOptionsBuilder();
            optBuilder.UseInMemoryDatabase("DoctorWhoTestsDB");

            _queryContext = new DoctorWhoCoreDbContext(optBuilder.Options);
            _entitiesContext = new DoctorWhoCoreDbContext(optBuilder.Options);
            _disconnectedContext = new DoctorWhoCoreDbContext(optBuilder.Options);

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
    }
}