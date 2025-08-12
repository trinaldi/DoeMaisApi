using DoeMais.Controllers.User;
using DoeMais.DTOs.User;
using DoeMais.Domain.Entities;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using DoeMais.Common;
using DoeMais.Domain.Enums;
using DoeMais.Exceptions;
using DoeMais.Tests.Helpers;
using DoeMais.Tests.Helpers.Asserts;

namespace DoeMais.Tests.Controllers;

public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private UserController _userController;
    private User _user;
    private UserProfileDto _userProfileDto;

    [SetUp]
    public void Setup()
    {
        _user = FakeUser.Create().ToEntity();
        _userProfileDto = _user.ToDto();
        _userServiceMock = new Mock<IUserService>();
        _userController = new UserController(_userServiceMock.Object);
        
        var claimUser = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, _user.UserId.ToString())
        ], "mock"));

        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimUser }
        };
    }

    [Test]
    public async Task GetMe_ReturnsUser_WhenUserExistsAsync()
    {
        _userServiceMock.Setup(service => service.GetByIdAsync(_userProfileDto.UserId))
            .ReturnsAsync(new Result<UserProfileDto?>(ResultType.Success, _userProfileDto));

        var result = await _userController.Get();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as Result<UserProfileDto?>;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            Assert.That(_userProfileDto, Is.EqualTo(resultDto?.Data).Using(new RecordDeepEqualityComparer<UserProfileDto>()));
        });
        
    }
    
    [Test]
    public async Task GetMe_ReturnsNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock.Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync(new Result<UserProfileDto?>(ResultType.NotFound));

        var result = await _userController.Get();

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdateProfile_ShouldReturnOk_WhenUpdateSucceeds()
    {
        var dto = new UpdateUserDto { Name = $"{Guid.NewGuid().ToString()}" };
        var resultFromService = new Result<UserProfileDto?>(ResultType.Success, new UserProfileDto { Name = dto.Name });

        _userServiceMock.Setup(s => s.UpdateUserAsync(_userProfileDto.UserId, dto))
            .ReturnsAsync(resultFromService);

        var actionResult = await _userController.Update(dto);
        var okResult = actionResult as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            Assert.That(okResult?.Value, Is.EqualTo(resultFromService));
        });
    }

    [Test]
    public async Task UpdateProfile_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var dto = new UpdateUserDto { Name = $"{Guid.NewGuid().ToString()}" };
        var notFoundResult = new Result<UserProfileDto?>(ResultType.NotFound, null, "User not found."); 
        
        _userServiceMock.Setup(s => s.UpdateUserAsync(_userProfileDto.UserId, dto))
            .ReturnsAsync(notFoundResult);

        
        var actionResult = await _userController.Update(dto);
        var notFoundObjectResult = actionResult as NotFoundObjectResult;
        
        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(notFoundObjectResult?.Value, Is.EqualTo(notFoundResult));
        });


    }
}