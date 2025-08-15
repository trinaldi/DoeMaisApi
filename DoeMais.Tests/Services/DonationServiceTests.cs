using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Donation;
using DoeMais.Extensions;
using DoeMais.Repositories;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Asserts;
using DoeMais.Tests.Helpers.Factories;
using Moq;

namespace DoeMais.Tests.Services;

public class DonationServiceTests
{
    private Mock<IDonationRepository> _donationRepositoryMock;
    private DonationService _donationService;

    [SetUp]
    public void Setup()
    {
        _donationRepositoryMock = new Mock<IDonationRepository>();
        _donationService = new DonationService(_donationRepositoryMock.Object);
    }

    [Test]
    public async Task GetDonationByIdAsync_ShouldReturnDonation_WhenDonationExists()
    {
        var comparer = new RecordDeepEqualityComparer<DonationDto>();
        var donation = FakeDonation.Create().WithAddress().ToEntity();
        var donationDto = donation.ToDto();
        _donationRepositoryMock.Setup(r => r.GetDonationByIdAsync(donationDto.DonationId))
            .ReturnsAsync(donation);

        var result = await _donationService.GetDonationByIdAsync(donationDto.DonationId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(donationDto).Using(comparer));
        });
    }
    
    [Test]
    public async Task GetDonationByIdAsync_ShouldNotReturnDonation_WhenDonationDoesNotExist()
    {
        _donationRepositoryMock.Setup(r => r.GetDonationByIdAsync(1))
            .ReturnsAsync((Donation)null);

        var result = await _donationService.GetDonationByIdAsync(1);
        
        Assert.That(result.Data, Is.Null);
    }

    [Test]
    public async Task GetDonationListAsync_ShouldReturnListOfDonations_WhenDonationsExists()
    {
        var donations = Enumerable.Range(1, 3)
            .Select(_ => FakeDonation.Create().WithAddress().ToEntity())
            .ToList();
        _donationRepositoryMock.Setup(r => r.GetDonationListAsync())
            .ReturnsAsync(donations);

        var result = await _donationService.GetDonationListAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Data, Has.Count.EqualTo(donations.Count));
        }); 
    }
    
    [Test]
    public async Task GetAddressesAsync_ShouldReturnAnEmptyList_WhenAddressesDoesNotExist()
    {
        _donationRepositoryMock.Setup(r => r.GetDonationListAsync())
            .ReturnsAsync([]);

        var result = await _donationService.GetDonationListAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Data, Is.Empty);
        });
    }
    
    [Test]
    public async Task CreateDonationAsync_ShouldCreateDonation_WhenDtoIsValid()
    {
        var comparer = new RecordDeepEqualityComparer<DonationDto>();
        var donation = FakeDonation.Create().WithAddress().ToEntity();
        var createDonationDto = donation.ToCreateDonationDto();
        _donationRepositoryMock
            .Setup(r => r.CreateDonationAsync(It.IsAny<Donation>()))
            .ReturnsAsync((Donation a) => a);

        var result = await _donationService.CreateDonationAsync(createDonationDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Data!.UserId, Is.EqualTo(createDonationDto.UserId));
            Assert.That(result!.Data!.Title, Is.EqualTo(createDonationDto.Title));
            Assert.That(result!.Data!.Description, Is.EqualTo(createDonationDto.Description));
            Assert.That(result!.Data!.Quantity, Is.EqualTo(createDonationDto.Quantity));
            Assert.That(result!.Data!.Status, Is.EqualTo(createDonationDto.Status));
            Assert.That(result!.Data!.Images, Is.EquivalentTo(createDonationDto.Images ?? new List<string>()));
        });
    }
   
    [Test]
    public async Task CreateDonationAsync_ShouldThrowArgumentNullException_WhenDtoTitleIsEmpty()
    {
        var donation = new Donation { Title = string.Empty };
        var donationDto = donation.ToCreateDonationDto();
        
        Assert.ThrowsAsync<ArgumentNullException>(async() => await _donationService
            .CreateDonationAsync(donationDto));
    }
    
    [Test]
    public async Task UpdateDonationAsync_ShouldUpdateDonation_WhenSomePropIsChanged()
    {
        var donation = FakeDonation.Create().WithAddress().ToEntity();
        donation.DonationId = 1;
        var donationInRepo = donation.Clone();
        var newDescription = FakeDonation.Create().Description;
        var donationDto = new UpdateDonationDto { DonationId = donation.DonationId, Description = newDescription };
        
        _donationRepositoryMock.Setup(r => r.GetDonationByIdAsync(It.Is<long>(id => id == donationInRepo.DonationId)))
            .ReturnsAsync(donation);
        _donationRepositoryMock.Setup(r => r.UpdateDonationAsync(donationDto.DonationId, It.IsAny<Donation>()))
            .ReturnsAsync((long donationId, Donation d) => d);
        
        await _donationService.UpdateDonationAsync(donationInRepo.DonationId, donationDto);
        
        _donationRepositoryMock
            .Verify(r => r.UpdateDonationAsync(
                It.Is<long>(id => id == donationDto.DonationId),         
                It.Is<Donation>(a => 
                    a.DonationId == donationDto.DonationId &&
                    a.Description == donationDto.Description
                )), Times.Once);
    }

    [Test]
    public async Task UpdateDonationAsync_ShouldReturnMismatch_WhenIdsAreNotTheSame()
    {
        var bogusDonationId = 2;
        var donationToBeUpdated = FakeDonation.Create().WithAddress().ToDto();
        var donationDto = new UpdateDonationDto { DonationId = donationToBeUpdated.DonationId };
        
        var result = await _donationService.UpdateDonationAsync(bogusDonationId, donationDto);
        
        _donationRepositoryMock
            .Verify(r => r.UpdateDonationAsync(
                It.Is<long>(id => id == donationDto.DonationId),         
                It.Is<Donation>(a => 
                    a.DonationId == donationDto.DonationId &&
                    a.Description == donationDto.Description
                )), Times.Never);
        Assert.Multiple(() =>
        {
            Assert.That(result.Type, Is.EqualTo(ResultType.Mismatch));
            Assert.That(result.Data, Is.Null); 
        });

    }
    
    [Test]
    public async Task DeleteDonationAsync_ShouldHardDeleteDonationReturningTrue_WhenDonationIdIsValid()
    {
        var donation = FakeDonation.Create().WithAddress().ToEntity();
        var donationDto = donation.ToDto();
        _donationRepositoryMock.Setup(r => r.DeleteDonationAsync(donationDto.DonationId))
            .ReturnsAsync(true);
        
        var result = await _donationService.DeleteDonationAsync(donationDto.DonationId); 
        
        Assert.That(result.Data, Is.EqualTo(true));
    }
    
    [Test]
    public async Task DeleteDonationAsync_ShouldNotHardDeleteDonationReturningFalse_WhenDonationIdIsInvalid()
    {
        var bogusDonationId = 1;
        _donationRepositoryMock.Setup(r => r.DeleteDonationAsync(bogusDonationId))
            .ReturnsAsync(false);

        var result = await _donationService.DeleteDonationAsync(bogusDonationId);
        
        Assert.That(result.Data, Is.EqualTo(false));
    }

}