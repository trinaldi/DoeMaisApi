using Microsoft.EntityFrameworkCore;
using DoeMais.Repositories;
using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Repositories.Interfaces;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;

namespace DoeMais.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private AppDbContext _context;
        private DbContextOptions<AppDbContext> _options;
        private IUserRepository _userRepository;
        private User _user;

        [SetUp]
        public async Task Setup()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DoeMaisTestDb_{System.Guid.NewGuid()}")
                .Options;
            _context = new AppDbContext(_options);
            _userRepository = new UserRepository(_context);
            _user = FakeUser.CreateFakeUser().ToUser();
            await _context.Users.AddAsync(_user);
            await _context.SaveChangesAsync();
            
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var result = await _userRepository.GetByIdAsync(_user.UserId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(_user));
                
            });
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _userRepository.GetByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task PutAsync_ShouldUpdateUser_WhenNameIsChanged()
        {
            var newName = Guid.NewGuid().ToString();
            _user.Name = newName;
            await _userRepository.UpdateAsync(_user);
            
            _context.ChangeTracker.Clear();
            
            var updatedUser = await _userRepository.GetByIdAsync(_user.UserId);
            
            Assert.Multiple(() =>
            {
                Assert.That(updatedUser, Is.Not.Null);
                Assert.That(updatedUser!.UserId, Is.EqualTo(_user.UserId));
                Assert.That(updatedUser!.Name, Is.EqualTo(newName));
            });
        }
    }
}
