using Microsoft.EntityFrameworkCore;
using DoeMais.Repositories;
using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services.Query;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using Moq;

namespace DoeMais.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private AppDbContext _context;
        private DbContextOptions<AppDbContext> _options;
        private IUserRepository _userRepository;
        private Mock<ICurrentUserService>_mockCurrentUserService;
        private User _user;

        [SetUp]
        public async Task Setup()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockCurrentUserService.Setup(m => m.UserId).Returns(1);
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DoeMaisTestDb_{System.Guid.NewGuid()}")
                .Options;
            _context = new AppDbContext(_options, _mockCurrentUserService.Object);
            _userRepository = new UserRepository(_context);
            _user = FakeUser.Create().ToEntity();
            await _context.Users.AddAsync(_user);
            await _context.SaveChangesAsync();
            
            _context.Entry(_user).State = EntityState.Detached;
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
                Assert.That(result?.UserId, Is.EqualTo(_user.UserId));
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
            var userToUpdate = _user.Clone();
            
            var newName = Guid.NewGuid().ToString();
            userToUpdate.Name = newName;
            
            await _userRepository.UpdateAsync(userToUpdate);
            _context.ChangeTracker.Clear();
            var updatedUser = await _userRepository.GetByIdAsync(userToUpdate.UserId);
            
            Assert.Multiple(() =>
            {
                Assert.That(updatedUser, Is.Not.Null);
                Assert.That(updatedUser!.UserId, Is.EqualTo(userToUpdate.UserId));
                Assert.That(updatedUser!.Name, Is.EqualTo(newName));
            });
        }
    }
}
