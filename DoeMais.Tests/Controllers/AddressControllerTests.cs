using System.Security.Claims;
using DoeMais.Common;
using DoeMais.Controllers.Address;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
using DoeMais.DTOs.User;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Asserts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DoeMais.Tests.Controllers;

public class AddressControllerTests
{
    private Mock<IAddressService> _addressServiceMock;
    private AddressController _addressController;

    [SetUp]
    public void Setup()
    {
        _addressServiceMock = new Mock<IAddressService>();
        _addressController = new AddressController(_addressServiceMock.Object);

        var user = FakeUser.Create().ToEntity();
        var claimUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        ], "mock"));

        _addressController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimUser }
        };
    }

    [Test]
    public async Task GetAddressByIdAsync_ShouldReturnResultSuccess_WhenAddressExist()
    {
        var address = FakeAddress.Create().ToEntity();
        var addressDto = address.ToDto();
        var addressId = addressDto.AddressId;
        _addressServiceMock.Setup(s => s.GetAddressByIdAsync(addressId))
            .ReturnsAsync(new Result<AddressDto>(ResultType.Success, addressDto));
       
        var result = await _addressController.GetAddressById(addressId);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<AddressDto>;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data, Is.EqualTo(addressDto).Using(new RecordDeepEqualityComparer<AddressDto>()));
        });

    }
    [Test]
    public async Task GetAddressByIdAsync_ShouldReturnNull_WhenAddressIdIsInvalid()
    {
        _addressServiceMock.Setup(s => s.GetAddressByIdAsync(1))
            .ReturnsAsync(new Result<AddressDto>(ResultType.NotFound));
       
        var result = await _addressController.GetAddressById(1);
        var okObjectResult = (NotFoundObjectResult)result;
        var resultDto = okObjectResult.Value as Result<AddressDto>;
            
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(resultDto?.Data, Is.Null);
        });
    }

    [Test]
    public async Task GetAddressesAsync_ShouldReturnAddresses_WhenAddressesExist()
    {
        var fakeAddresses = FakeAddress.CreateMany(qty: 3);
        var addresses = fakeAddresses.Select(a => a.ToDto()).ToList();
        _addressServiceMock.Setup(s => s.GetAddressesAsync())
            .ReturnsAsync(new Result<List<AddressDto>?>(ResultType.Success, addresses));
        
        var result = await _addressController.GetAddresses();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<List<AddressDto>>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data?.Count, Is.EqualTo(addresses.Count));
        });
    }

    [Test]
    public async Task GetAddressesAsync_ShouldReturnEmptyList_WhenAddressesDoNotExist()
    {
        var addresses = new List<AddressDto>();
        _addressServiceMock.Setup(s => s.GetAddressesAsync())
            .ReturnsAsync(new Result<List<AddressDto>?>(ResultType.Success, addresses));
        
        var result = await _addressController.GetAddresses();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<List<AddressDto>>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data?.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task CreateAddressAsync_ShouldCreateAddress_WhenDtoExists()
    {
        var addressToBeCreated = FakeAddress.Create().ToDto();
        _addressServiceMock.Setup(s => s.CreateAddressAsync(addressToBeCreated))
            .ReturnsAsync(new Result<AddressDto?>(ResultType.Success, addressToBeCreated));
        
        var result = await _addressController.CreateAddress(addressToBeCreated);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<AddressDto>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data,
                Is.EqualTo(addressToBeCreated).Using(new RecordDeepEqualityComparer<AddressDto>()));
        });
    }
    
    [Test]
    public async Task CreateAddressAsync_ShouldNotCreateAddress_WhenStreetIsNull()
    {
        var addressToBeCreated = new AddressDto() { Street = null };
        _addressServiceMock.Setup(s => s.CreateAddressAsync(addressToBeCreated))
            .ReturnsAsync(new Result<AddressDto?>(ResultType.Error));
        
        var result = await _addressController.CreateAddress(addressToBeCreated);
        var okObjectResult = (BadRequestObjectResult)result;
        var resultDto = okObjectResult.Value as Result<AddressDto>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        });
    }

    [Test]
    public async Task DeleAddressAsync_ShouldHardDeleteAddress_WhenAddressIdExists()
    {
        var addressToBeDeleted = FakeAddress.Create().AddressId;
        _addressServiceMock.Setup(s => s.DeleteAddressAsync(addressToBeDeleted))
            .ReturnsAsync(new Result<bool>(ResultType.Success, true));
        
        var result = await _addressController.DeleteAddress(addressToBeDeleted);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<bool>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultDto?.Data, Is.True);
        });
    }
    
    [Test]
    public async Task DeleAddressAsync_ShouldNotHardDeleteAddress_WhenAddressIdDoesNotExists()
    {
        var addressToBeDeleted = FakeAddress.Create().AddressId;
        _addressServiceMock.Setup(s => s.DeleteAddressAsync(addressToBeDeleted))
            .ReturnsAsync(new Result<bool>(ResultType.Error, false));
        
        var result = await _addressController.DeleteAddress(addressToBeDeleted);
        var okObjectResult = (NotFoundObjectResult)result;
        var resultDto = okObjectResult.Value as Result<bool>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(resultDto?.Data, Is.False);
        });
    }
    

}