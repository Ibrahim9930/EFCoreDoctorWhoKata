using System.Linq;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DoctorWho.Tests
{
    public class CRUDTests
    {
        private DoctorWhoCoreDbContext _context;

        public CRUDTests()
        {
            DbContextOptionsBuilder optBuilder = new DbContextOptionsBuilder();
            optBuilder.UseInMemoryDatabase("testing_database");
            _context = new DoctorWhoCoreDbContext(optBuilder.Options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public void Should_AddAuthor()
        {
            string authorName = "new author";
            Author author = new Author(_context)
            {
                AuthorName = authorName
            };
            
            author.Add();

            _context.Authors.Where(a => a.AuthorName == authorName).Should().NotBeEmpty();
        }
        
        [Fact]
        public void Should_UpdateAuthor()
        {
            string newName = "changed name";
            Author author = _context.Authors.Find(1);
            author.Context = _context;
            
            author.AuthorName = newName;
            author.Update();

            _context.Authors.Find(1).AuthorName.Should().Be(newName);
        }
        
        [Fact]
        public void Should_DeleteAuthor()
        {
            Author author = _context.Authors.Find(1);
            author.Context = _context;
            
            author.Delete();

            _context.Authors.Find(1).Should().BeNull();
        }
    }
}