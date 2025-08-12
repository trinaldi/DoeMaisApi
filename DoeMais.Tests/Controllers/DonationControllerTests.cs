using System.Security.Claims;
using DoeMais.Common;
using DoeMais.Controllers.Address;
using DoeMais.Controllers.Donation;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Address;
using DoeMais.DTOs.Donation;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers.Asserts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DoeMais.Tests.Controllers;

public class DonationControllerTests
{
    private Mock<IDonationService> _donationServiceMock;
    private DonationController _donationController;

    [SetUp]
    public void Setup()
    {
        _donationServiceMock = new Mock<IDonationService>();
        _donationController = new DonationController(_donationServiceMock.Object);

        var user = FakeUser.Create().ToEntity();
        var claimUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        ], "mock"));

        _donationController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimUser }
        };
    }

    [Test]
    public async Task GetDonationById_ShouldReturnResultSuccess_WhenDonationExists()
    {
        var donationDto = FakeDonation.Create().WithAddress().ToDto();
        var donationId = donationDto.DonationId;
        _donationServiceMock.Setup(s => s.GetDonationByIdAsync(donationId))
            .ReturnsAsync(new Result<DonationDto?>(ResultType.Success, donationDto));

        var result = await _donationController.GetById(donationId);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<DonationDto>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data, Is.EqualTo(donationDto).Using(new RecordDeepEqualityComparer<DonationDto>()));
        });
    }

    [Test]
    public async Task GetDonationById_ShouldReturnNotFound_WhenDonationDoesNotExist()
    {
        const int bogusDonationId = 1;
        _donationServiceMock.Setup(s => s.GetDonationByIdAsync(bogusDonationId))
            .ReturnsAsync(new Result<DonationDto?>(ResultType.NotFound, null, "Donation not found."));
        
        var result = await _donationController.GetById(bogusDonationId);
        var notFoundObjectResult = (NotFoundObjectResult)result;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(notFoundObjectResult.Value, Is.EqualTo("Donation not found."));
        });
    }

    [Test]
    public async Task GetDonationList_ShouldReturnDonation_WhenDonationsExist()
    {
        var donations = FakeDonation.CreateMany(3).Select(d => d.ToDto()).ToList();
        _donationServiceMock.Setup(s => s.GetDonationListAsync())
            .ReturnsAsync(new Result<List<DonationDto>>(ResultType.Success, donations));
        
        var result = await _donationController.GetAll();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<List<DonationDto>>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data, Is.EqualTo(donations).Using(new RecordDeepEqualityComparer<DonationDto>()));
        });
    }

    [Test]
    public async Task GetDonationList_ShouldReturnAnEmptyList_WhenDonationsDoNotExist()
    {
        var donations = new List<DonationDto>();
        _donationServiceMock.Setup(s => s.GetDonationListAsync())
            .ReturnsAsync(new Result<List<DonationDto>>(ResultType.Success, donations));
        
        var result = await _donationController.GetAll();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<List<DonationDto>>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data?.Count, Is.EqualTo(donations.Count));
        });
    }
    
    [Test]
    public async Task CreateDonation_ShouldCreateDonation_WhenDtoExist()
    {
        var toBeCreated = FakeDonation.Create().WithAddress().ToDto();
        var toBeCreatedDto = new CreateDonationDto
        {
            Title = toBeCreated.Title!
        };
        _donationServiceMock.Setup(s => s.CreateDonationAsync(toBeCreatedDto))
            .ReturnsAsync(new Result<DonationDto?>(ResultType.Success, toBeCreated));

        var result = await _donationController.Create(toBeCreatedDto);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<DonationDto>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(resultDto?.Data,
                Is.EqualTo(toBeCreated).Using(new RecordDeepEqualityComparer<DonationDto>()));
        });
    }
    
    [Test]
    public async Task UpdateDonationAsync_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        const long bogusDonationId = 1L;
        var donationDto = new UpdateDonationDto { DonationId = 2L };
        _donationServiceMock.Setup(s => s.UpdateDonationAsync(bogusDonationId, donationDto))
            .ReturnsAsync(new Result<DonationDto?>(ResultType.Mismatch, null, "Donation Ids mismatch."));

        var result = await _donationController.Update(bogusDonationId, donationDto);
        var badRequestObjectResult = (BadRequestObjectResult)result;
        var resultDto = badRequestObjectResult.Value as Result<DonationDto>;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(resultDto, Is.Not.Null);
            Assert.That(resultDto!.Type, Is.EqualTo(ResultType.Mismatch));
            Assert.That(resultDto.Message, Does.Contain("mismatch"));
            Assert.That(badRequestObjectResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        });
    }
    
    [Test]
    public async Task UpdateDonationAsync_ShouldReturnOk_WhenUpdateSucceeds()
    {
        var donationId = 1L;
        var donationDto = FakeDonation.Create().WithAddress().ToDto();
        var dto = new UpdateDonationDto { DonationId = donationId, Description = donationDto.Description };
        var successResult = new Result<DonationDto?>(ResultType.Success, donationDto);
        _donationServiceMock.Setup(s => s.UpdateDonationAsync(donationId, dto))
            .ReturnsAsync(successResult);

        var result = await _donationController.Update(donationId, dto);
        var okResult = (OkObjectResult)result;
        var value = okResult.Value as Result<DonationDto?>;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(value, Is.Not.Null);
            Assert.That(value!.Type, Is.EqualTo(ResultType.Success));
            Assert.That(value.Data, Is.Not.Null);
        });
    }

    [Test]
    public async Task UpdateDonationAsync_ShouldReturnNotFound_WhenDonationNotFoundOrError()
    {
        const long donationId = 1L;
        var dto = new UpdateDonationDto { DonationId = donationId };

        var notFoundResult = new Result<DonationDto?>(ResultType.NotFound, null, "Donation not found.");
        _donationServiceMock.Setup(s => s.UpdateDonationAsync(donationId, dto))
            .ReturnsAsync(notFoundResult);

        var result = await _donationController.Update(donationId, dto);
        var notFound = (NotFoundObjectResult)result;
        var val = notFound.Value as Result<DonationDto?>;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(notFound.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(val, Is.Not.Null);
            Assert.That(val!.Type, Is.EqualTo(ResultType.NotFound));
        });
    }
    
    [Test]
    public async Task DeleteDonation_ShouldHardDeleteDonation_WhenDonationIdExists()
    {
        var toBeDeleted = FakeDonation.Create().WithAddress().DonationId;
        _donationServiceMock.Setup(s => s.DeleteDonationAsync(toBeDeleted))
            .ReturnsAsync(new Result<bool>(ResultType.Success, true));
        
        var result = await _donationController.Delete(toBeDeleted);
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<bool>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultDto?.Data, Is.True);
        });
    }
    
    [Test]
    public async Task DeleteDonation_ShouldNotHardDeleteDonation_WhenDonationIdDoesNotExist()
    {
        const long bogusDonationId = 1L;
        _donationServiceMock.Setup(s => s.DeleteDonationAsync(bogusDonationId))
            .ReturnsAsync(new Result<bool>(ResultType.Error, false));
        
        var result = await _donationController.Delete(bogusDonationId);
        var okObjectResult = (NotFoundObjectResult)result;
        var resultDto = okObjectResult.Value as Result<bool>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(resultDto?.Data, Is.False);
        });
    }
    
}