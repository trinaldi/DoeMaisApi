using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Asserts;
using Moq;

namespace DoeMais.Tests.Services;

public class AddressServiceTests
{
    private Mock<IAddressRepository> _addressRepositoryMock;
    private AddressService _addressService;

    [SetUp]
    public void Setup()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _addressService = new AddressService(_addressRepositoryMock.Object);
    }

    [Test]
    public async Task GetAddressById_ShouldReturnAddress_WhenAddressExist()
    {
        var comparer = new RecordDeepEqualityComparer<AddressDto>();
        var address = FakeAddress.Create().ToEntity();
        var addressDto = address.ToDto();
        _addressRepositoryMock.Setup(r => r.GetAddressByIdAsync(addressDto.AddressId))
            .ReturnsAsync(address);

        var result = await _addressService.GetAddressByIdAsync(addressDto.AddressId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(addressDto).Using(comparer));
        });
    }

    [Test]
    public async Task GetAddressById_ShouldNotReturnAddress_WhenAddressDoesNotExist()
    {
        _addressRepositoryMock.Setup(r => r.GetAddressByIdAsync(1))
            .ReturnsAsync((Address)null);

        var result = await _addressService.GetAddressByIdAsync(1);
        
        Assert.That(result.Data, Is.Null);
    }

    [Test]
    public async Task GetAddressesAsync_ShouldReturnAllAddresses_WhenAddressesExists()
    {
        var addresses = 
            Enumerable.Range(0, 10)
                .Select(_ => FakeAddress.Create().ToEntity())
                .ToList();
        _addressRepositoryMock.Setup(r => r.GetAddressesAsync())
            .ReturnsAsync(addresses);

        var result = await _addressService.GetAddressesAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Has.Count.EqualTo(addresses.Count));
        });
    }

    [Test]
    public async Task GetAddressesAsync_ShouldReturnAnEmptyList_WhenAddressesDoesNotExist()
    {
        _addressRepositoryMock.Setup(r => r.GetAddressesAsync())
            .ReturnsAsync([]);

        var result = await _addressService.GetAddressesAsync();
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Empty);
        });
    }

    [Test]
    public async Task CreateAddressAsync_ShouldCreateAddress_WhenDataIsValid()
    {
        var comparer = new RecordDeepEqualityComparer<AddressDto>();
        var address = FakeAddress.Create().ToEntity();
        var addressDto = address.ToDto();
        _addressRepositoryMock
            .Setup(r => r.CreateAddressAsync(It.IsAny<Address>()))
            .ReturnsAsync((Address a) => a);

        var result = await _addressService.CreateAddressAsync(addressDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(addressDto).Using(comparer));
        });
    }

    [Test]
    public async Task CreateAddressAsync_ShouldThrowArgumentNullException_WhenDtoStreetIsEmpty()
    {
        var address = new Address { Street = string.Empty };
        var addressDto = address.ToDto();
        
        Assert.ThrowsAsync<ArgumentNullException>(async() => await _addressService.CreateAddressAsync(addressDto));
    }
    
    [Test]
    public async Task UpdateAddressAsync_ShouldUpdateAddress_WhenSomePropIsChanged()
    {
        var address = FakeAddress.Create().ToEntity();
        var addressInRepo = address.Clone();
        var newStreet = FakeAddress.Create().Street;
        var addressDto = new AddressDto { AddressId = address.AddressId, Street = newStreet };
        _addressRepositoryMock.Setup(r => r.UpdateAddressAsync(addressDto.AddressId, It.IsAny<Address>()))
            .ReturnsAsync((long addressId, Address a) => a);
        
        await _addressService.UpdateAddressAsync(addressInRepo.AddressId, addressDto);
        
        _addressRepositoryMock
            .Verify(r => r.UpdateAddressAsync(
                It.Is<long>(id => id == addressDto.AddressId),         
                It.Is<Address>(a => 
                    a.AddressId == addressDto.AddressId &&
                    a.Street == addressDto.Street
                )), Times.Once);
    }

    [Test]
    public async Task DeleteAddressAsync_ShouldHardDeleteAddressReturningTrue_WhenAddressIdIsValid()
    {
        var address = FakeAddress.Create().ToEntity();
        var addressDto = address.ToDto();
        _addressRepositoryMock.Setup(r => r.DeleteAddressAsync(addressDto.AddressId))
            .ReturnsAsync(true);
        
        var result = await _addressService.DeleteAddressAsync(addressDto.AddressId); 
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(true));
        });
    }
    
    [Test]
    public async Task DeleteAddressAsync_ShouldNotHardDeleteAndReturnFalse_WhenAddressIdIsNotValid()
    {
        _addressRepositoryMock.Setup(r => r.DeleteAddressAsync(1))
            .ReturnsAsync(false);
        
        var result = await _addressService.DeleteAddressAsync(1); 
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(false));
        });
    }

    [Test]
    public async Task CreateAddressAsync_ShouldSetIsPrimary_WhenAPrimaryAddressIsInserted()
    {
        var primaryAddress = FakeAddress.Create();
        primaryAddress.IsPrimary = true;
        var primaryAddressDto = primaryAddress.ToDto();
        _addressRepositoryMock.Setup(r => r.ClearPrimaryAddressAsync())
            .Returns(Task.CompletedTask);
        _addressRepositoryMock.Setup(r => r.CreateAddressAsync(It.IsAny<Address>()))
            .ReturnsAsync(new Address());

        var result = await _addressService.CreateAddressAsync(primaryAddressDto);
        
        Assert.That(result.Type, Is.EqualTo(ResultType.Success));
        _addressRepositoryMock.Verify(r => r.ClearPrimaryAddressAsync(), Times.Once);
        _addressRepositoryMock.Verify(r => r.CreateAddressAsync(It.IsAny<Address>()), Times.Once);

    }
    
}