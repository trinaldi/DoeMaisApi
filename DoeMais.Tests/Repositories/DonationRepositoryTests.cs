using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Extensions;
using DoeMais.Repositories;
using DoeMais.Services.Query;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Factories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DoeMais.Tests.Repositories;

[TestFixture]
public class DonationRepositoryTests
{
    private ICurrentUserService _currentUserService;
    private AppDbContext _context;
    private DonationRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _currentUserService = Mock.Of<ICurrentUserService>(s => s.UserId == 1);
        _context = TestDbContextFactory.CreateInMemoryContext(_currentUserService);
        _repository = new DonationRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetDonationByIdAsync_ShouldReturnDonation_WhenDonationExists()
    {
        var (_, donation) = await TestDataFactory.CreateUserWithDonationAsync(_context);

        var result = await _repository.GetDonationByIdAsync(donation.DonationId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.DonationId, Is.EqualTo(donation.DonationId));
            Assert.That(result?.UserId, Is.EqualTo(donation.UserId));
        });

    }

    [Test]
    public async Task GetDonationsById_ShouldReturnNull_WhenDonationDoesNotExist()
    {
        const int bogusDonationId = 1;

        var result = await _repository.GetDonationByIdAsync(bogusDonationId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDonationList_ShouldReturnAList_WhenDonationsExist()
    {
        var donations = await TestDataFactory.CreateDonationListAsync(_context);

        var result = await _repository.GetDonationListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Count, Is.EqualTo(donations.Count));
        });
    }

    [Test]
    public async Task GetDonationListAsync_ShouldReturnEmptyList_WhenNoDonationExist()
    {
        var result = await _repository.GetDonationListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result?.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task CreateDonationAsync_ShouldReturnDonation_WhenSuccessful()
    {
        var donation = FakeDonation.Create().WithAddress().ToEntity();

        var result = await _repository.CreateDonationAsync(donation);

        Assert.Multiple(() =>
        {
            Assert.That(result!.UserId, Is.EqualTo(donation.UserId));
            Assert.That(result!.DonationId, Is.EqualTo(donation.DonationId));
            Assert.That(result!.Title, Is.EqualTo(donation.Title));
            Assert.That(result!.Description, Is.EqualTo(donation.Description));
            Assert.That(result!.Quantity, Is.EqualTo(donation.Quantity));
            Assert.That(result!.Status, Is.EqualTo(donation.Status));
            Assert.That(result!.Images, Is.EqualTo(donation.Images));
        });
    }

    [Test]
    public async Task CreateDonationAsync_ShouldThrowException_WhenDatabaseFails()
    {
        var mockContext = new Mock<AppDbContext>();
        var mockDbSet = new Mock<DbSet<Donation>>();
        mockContext.Setup(m => m.Donations).Returns(mockDbSet.Object);
        mockDbSet.Setup(m => m.Add(It.IsAny<Donation>())).Verifiable();
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        var repository = new DonationRepository(mockContext.Object);
        var donation = FakeDonation.Create().WithAddress().ToEntity();

        Assert.ThrowsAsync<Exception>(async () => await repository.CreateDonationAsync(donation));
    }

    [Test]
    public async Task UpdateDonationAsync_ShouldUpdateDonation_WhenDonationExists()
    {
        var donation = await TestDataFactory.CreatePersistedDonationAsync(_context);
        var donationToBeUpdated = donation.Clone();
        var newDescription = FakeDonation.Create().Description;
        donationToBeUpdated.Description = newDescription;

        var result = await _repository.UpdateDonationAsync(donation.DonationId,
            donationToBeUpdated);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.DonationId, Is.EqualTo(donation.DonationId));
            Assert.That(result.Description, Is.EqualTo(newDescription));
        });
    }

    [Test]
    public async Task UpdateDonationAsync_ShouldNotUpdateDonation_WhenDonationDoesNotExist()
    {
        var bogusDonation = FakeDonation.Create().WithAddress().ToEntity();
        var newDescription = FakeDonation.Create().WithAddress().ToEntity().Description;
        var donationToBeUpdated = bogusDonation.Clone();
        donationToBeUpdated.Description = newDescription;

        var result = await _repository.UpdateDonationAsync(bogusDonation.DonationId, donationToBeUpdated);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteDonationAsync_ShouldDeleteDonationFromDatabase_WhenDonationExists()
    {
        var donation = await TestDataFactory.CreatePersistedDonationAsync(_context);
        var existingDonationNumber = await _context.Donations.CountAsync();

        await _repository.DeleteDonationAsync(donation.DonationId);
        var updatedDonationNumber = await _context.Donations.CountAsync();

        Assert.That(updatedDonationNumber, Is.EqualTo(existingDonationNumber - 1));
    }

    [Test]
    public async Task DeleteDonationAsync_ShouldReturnTrue_WhenDeletionIsCompleted()
    {
        var donation = await TestDataFactory.CreatePersistedDonationAsync(_context);

        var result = await _repository.DeleteDonationAsync(donation.DonationId);

        Assert.That(result, Is.True);
    }

     [Test]
     public async Task DeleteDonationAsync_ShouldReturnFalse_WhenDonationIsNotRemoved()
     {
         var bogusDonation = FakeDonation.Create().WithAddress().ToEntity();
         var donation = FakeDonation.Create().WithAddress().ToEntity();
         await _repository.CreateDonationAsync(donation);

         var result = await _repository.DeleteDonationAsync(bogusDonation.DonationId);

         Assert.That(result, Is.False);
     }

     [Test]
     public async Task DeleteDonationAsync_ShouldNotDeleteOtherAddresses_WhenDeletingSpecificAddress()
     {
         var firstDonation = FakeDonation.Create().WithAddress().ToEntity();
         firstDonation.Address.AddressId = 1;
         var secondDonation = FakeDonation.Create().WithAddress().ToEntity();
         firstDonation.Address.AddressId = 2;
         await _repository.CreateDonationAsync(firstDonation);
         await _repository.CreateDonationAsync(secondDonation);

         _context.ChangeTracker.Clear();
         await _repository.DeleteDonationAsync(secondDonation.DonationId);
         var firstDonationPresence = await _context.Donations.AnyAsync(a => a.DonationId == firstDonation.DonationId);
         var secondDonationPresence = await _context.Donations.AnyAsync(a => a.DonationId == secondDonation.DonationId);

         Assert.Multiple(() =>
         {
             Assert.That(firstDonationPresence, Is.True);
             Assert.That(secondDonationPresence, Is.False);
         });
     }
}