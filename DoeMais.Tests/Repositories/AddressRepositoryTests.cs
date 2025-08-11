using Microsoft.EntityFrameworkCore;
using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.DTOs.Address;
using DoeMais.Extensions;
using DoeMais.Repositories;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services.Query;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Factories;
using Moq;

namespace DoeMais.Tests.Repositories
{
    [TestFixture]
    public class AddressRepositoryTests
    {
        private ICurrentUserService _currentUserService;
        private AppDbContext _context;
        private AddressRepository _repository;

        [SetUp]
        public void Setup()
        {
            _currentUserService = Mock.Of<ICurrentUserService>(s => s.UserId == 1);
            _context = TestDbContextFactory.CreateInMemoryContext(_currentUserService);
            _repository = new AddressRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAddressByIdAsync_ShouldReturnAddress_WhenAddressExists()
        {
            var (_, address) = await TestDataFactory.CreateUserWithAddressAsync(_context);
            
            var result = await _repository.GetAddressByIdAsync(address.AddressId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.AddressId, Is.EqualTo(address.AddressId));
                Assert.That(result?.UserId, Is.EqualTo(address.UserId));
            });
        }

        [Test]
        public async Task GetAddressByIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            const int bogusAddressId = 1;
            
            var result = await _repository.GetAddressByIdAsync(bogusAddressId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAllAddressesAsync_ShouldReturnListOfAddresses_WhenAddressesExist()
        {
            var addresses = await TestDataFactory.CreateAddressesAsync(_context);

            var result = await _repository.GetAddressesAsync();
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.Count, Is.EqualTo(addresses.Count));
            });
        }
        
        [Test]
        public async Task GetAllAddressesAsync_ShouldReturnEmptyList_WhenNoAddressesExist()
        {
            var result = await _repository.GetAddressesAsync();
            
            Assert.Multiple(() =>
            {
                Assert.That(result?.Count, Is.EqualTo(0));
            });
        }
        
        [Test]
        public async Task CreateAddressAsync_ShouldAddAddress_AndCallSaveChangesAsync()
        {
            var existingAddressesNumber = await _context.Addresses.CountAsync();
            var address = FakeAddress.Create().ToEntity();
            
            var result = await _repository.CreateAddressAsync(address);
            var newAddressesNumber = await _context.Addresses.CountAsync();
           
            
            Assert.Multiple(() =>
            {
                Assert.That(newAddressesNumber, Is.EqualTo(existingAddressesNumber + 1));
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.Street, Is.EqualTo(address.Street));
            });
        }

        [Test]
        public async Task CreateAddressAsync_ShouldSetUserId_WhenAddressIsCreated()
        {
            var address = FakeAddress.Create().ToEntity();
            var userId = _currentUserService.UserId;
            
            var result = await _repository.CreateAddressAsync(address);
            
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        public async Task CreateAddressAsync_ShouldPersistAllFieldsCorrectly()
        {
            var address = FakeAddress.Create().WithDonation().ToEntity();
            
            var result = await _repository.CreateAddressAsync(address);

            Assert.Multiple(() =>
            {
                Assert.That(result.UserId, Is.EqualTo(address.UserId));
                Assert.That(result.Street, Is.EqualTo(address.Street));
                Assert.That(result.Complement, Is.EqualTo(address.Complement));
                Assert.That(result.City, Is.EqualTo(address.City));
                Assert.That(result.State, Is.EqualTo(address.State));
                Assert.That(result.ZipCode, Is.EqualTo(address.ZipCode));
                Assert.That(result.IsPrimary, Is.EqualTo(address.IsPrimary));
                Assert.That(result.Donations, Is.EqualTo(address.Donations));
            });

        }

        [Test]
        public async Task CreateAddressAsync_ShouldThrowException_WhenDatabaseFails()
        {
            var mockContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<Address>>();
            mockContext.Setup(m => m.Addresses).Returns(mockDbSet.Object);
            mockDbSet.Setup(m => m.Add(It.IsAny<Address>())).Verifiable();
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var repository = new AddressRepository(mockContext.Object);
            var address = FakeAddress.Create().ToEntity();
            
            Assert.ThrowsAsync<Exception>(async() => await repository.CreateAddressAsync(address));
        }

        [Test]
        public async Task UpdateAddressAsync_ShouldUpdateAddress_WhenAddressExists()
        {
            var address = await TestDataFactory.CreatePersistedAddressAsync(_context);
            var addressToUpdate = address.Clone();
            var newStreet = FakeAddress.Create().Street;
            addressToUpdate.Street = newStreet;
           
            var result = await _repository.UpdateAddressAsync(address.AddressId, addressToUpdate);
           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.AddressId, Is.EqualTo(address.AddressId));
                Assert.That(result.Street, Is.EqualTo(newStreet));
            });
        }

        [Test]
        public async Task UpdateAddressAsync_ShouldNotUpdateAddress_WhenAddressDoesNotExist()
        {
            var bogusAddress = FakeAddress.Create().ToEntity();
            var newStreet = FakeAddress.Create().ToEntity().Street;
            var addressToUpdate = bogusAddress.Clone();
            addressToUpdate.Street = newStreet;
            
            var result = await _repository.UpdateAddressAsync(bogusAddress.AddressId, addressToUpdate);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAddressAsync_ShouldRemoveAddressFromDatabase_WhenAddressExists()
        {
            var address = FakeAddress.Create().ToEntity();
            await _repository.CreateAddressAsync(address);
            var existingAddressesNumber = await _context.Addresses.CountAsync();
            
            await _repository.DeleteAddressAsync(address.AddressId);
            var updatedAddressesNumber = await _context.Addresses.CountAsync();
            
            Assert.That(updatedAddressesNumber, Is.EqualTo(existingAddressesNumber - 1));
        }

        [Test]
        public async Task DeleteAddressAsync_ShouldReturnTrue_WhenAddressIsDeleted()
        {
            var address = FakeAddress.Create().ToEntity();
            await _repository.CreateAddressAsync(address);
            
            var result = await _repository.DeleteAddressAsync(address.AddressId);
            
            Assert.That(result, Is.True);
        }
        
        [Test]
        public async Task DeleteAddressAsync_ShouldReturnFalse_WhenAddressIsNotDeleted()
        {
            var bogusAddress = FakeAddress.Create().ToEntity();
            var address = FakeAddress.Create().ToEntity();
            await _repository.CreateAddressAsync(address);
            
            var result = await _repository.DeleteAddressAsync(bogusAddress.AddressId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAddressAsync_ShouldNotDeleteOtherAddresses_WhenDeletingSpecificAddress()
        {
            var firstAddress = FakeAddress.Create().ToEntity();
            await _repository.CreateAddressAsync(firstAddress);
            var secondAddress = FakeAddress.Create().ToEntity();
            await _repository.CreateAddressAsync(secondAddress);
            
            await _repository.DeleteAddressAsync(secondAddress.AddressId);
            var firstAddressExists = await _context.Addresses.AnyAsync(a => a.AddressId == firstAddress.AddressId);
            var secondAddressExists = await _context.Addresses.AnyAsync(a => a.AddressId == secondAddress.AddressId);

            Assert.Multiple(() =>
            {
                Assert.That(firstAddressExists, Is.True);
                Assert.That(secondAddressExists, Is.False);
            });
        }

    }
}