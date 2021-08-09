using System.Linq;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using DoctorWho.Db.Repositories;
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
            
            AuthorEFRepository authorRepo = new AuthorEFRepository(_context);
            
            Author author = new Author()
            {
                AuthorName = authorName
            };
            
            authorRepo.Add(author);
            authorRepo.Commit();

            _context.Authors.Where(a => a.AuthorName == authorName).Should().NotBeEmpty();
        }
        
        [Fact]
        public void Should_UpdateAuthor()
        {
            AuthorEFRepository authorRepo = new AuthorEFRepository(_context);
            string newName = "changed name";

            Author author = _context.Authors.Find(1);
            
            author.AuthorName = newName;
            authorRepo.Update(author);
            authorRepo.Commit();

            _context.Authors.Find(1).AuthorName.Should().Be(newName);
        }
        
        [Fact]
        public void Should_DeleteAuthor()
        {
            AuthorEFRepository authorRepo = new AuthorEFRepository(_context);
            string newName = "changed name";

            Author author = _context.Authors.Find(1);
            
            authorRepo.Delete(author);
            authorRepo.Commit();

            _context.Authors.Find(1).Should().BeNull();
        }
    }
}