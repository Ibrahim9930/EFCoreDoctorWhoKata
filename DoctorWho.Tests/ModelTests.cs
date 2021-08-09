using System;
using DoctorWho.Db;
using FluentAssertions;
using Xunit;

namespace DoctorWho.Tests
{
    public class ModelTests
    {
        private readonly DoctorWhoCoreDbContext _context;

        public ModelTests()
        {
            _context = new DoctorWhoCoreDbContext();
        }

        [Fact] public void EpisodeEnemyJoinEntityShouldExist()
        {
            _context.Model.FindEntityType("EnemyEpisode").Should().NotBeNull();
        }
        
        [Fact]
        public void EpisodeCompanionJoinEntityShouldExist()
        {
            _context.Model.FindEntityType("CompanionEpisode").Should().NotBeNull();
        }
        
    }
}