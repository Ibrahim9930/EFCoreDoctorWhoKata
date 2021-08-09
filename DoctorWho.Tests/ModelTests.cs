using System;
using System.Collections;
using System.Collections.Generic;
using DoctorWho.Db;
using DoctorWho.Db.Utilities;
using FluentAssertions;
using Moq;
using Xunit;

namespace DoctorWho.Tests
{
    public class ModelTests
    {
        private readonly DoctorWhoCoreDbContext _context;

        public ModelTests()
        {
            var entityReaderMock = new Mock<IEntityReader>();
            entityReaderMock.Setup(er =>
                    er.ReadFakeEntity<It.IsAnyType>(It.IsAny<int>(), It.IsAny<Func<It.IsAnyType, int>>()))
                .Returns<It.IsAnyType>(null);
            entityReaderMock.Setup(er =>
                    er.ReadAllFakeEntities<It.IsAnyType>())
                .Returns<IEnumerator<It.IsAnyType>>(null);
            entityReaderMock.Setup(er =>
                    er.ReadAllFakeJoinEntities<It.IsAnyType,It.IsAnyType>())
                .Returns<IEnumerable>(null);
            
            _context = new DoctorWhoCoreDbContext(entityReaderMock.Object);
        }

        [Fact]
        public void EpisodeEnemyJoinEntityShouldExist()
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