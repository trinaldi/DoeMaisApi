using System.IdentityModel.Tokens.Jwt;
using DoeMais.Controllers.User;
using DoeMais.DTO.User;
using DoeMais.Domain.Entities;
using DoeMais.Extensions;
using DoeMais.Services.Interfaces;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using DoeMais.Exceptions;
using DoeMais.Tests.Assertions;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace DoeMais.Tests.Controllers;

public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private UserController _userController;
    private User _user;

    [SetUp]
    public void Setup()
    {
        _user = FakeUser.CreateFakeUser().ToUser();
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
        var profileUserDto = _user.ToDto();
        _userServiceMock.Setup(x => x.GetByIdAsync(_user.UserId)).ReturnsAsync(_user);

        var result = await _userController.Me();
        var okObjectResult = (OkObjectResult)result;
        var resultDto = okObjectResult.Value as UserProfileDto;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(okObjectResult.Value, Is.Not.Null);
            DtoAssertions.AssertAreEqual(profileUserDto, resultDto);
        });
        
    }
    
    [Test]
    public async Task GetMe_ReturnsNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);

        var result = await _userController.Me();

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task UpdateProfile_ShouldUpdateUser_WhenNameChanges()
    {
        var newName = Guid.NewGuid().ToString();
        var updateUserDto = new UpdateUserDto { Name = newName };
        var updatedUser = _user.Clone();
        updatedUser.Name = newName;
        
        _userServiceMock.Setup(x => x.UpdateUserAsync(_user.UserId, updateUserDto))
            .ReturnsAsync(updatedUser);
       
        var result = await _userController.UpdateProfile(updateUserDto);
        var resultDto = result as OkObjectResult;
       
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            DtoAssertions.AssertAreEqual(updateUserDto, resultDto?.Value);
        });
        _userServiceMock.Verify(x => x.UpdateUserAsync(_user.UserId, updateUserDto), Times.Once);
    }

    [Test]
    public async Task UpdateProfile_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var updateUserDto = _user.ToUpdateUserDto();
        _userServiceMock.Setup(s => s.UpdateUserAsync(It.IsAny<long>(), updateUserDto))
            .ThrowsAsync(new NotFoundException<User>());
        
        var result = await _userController.UpdateProfile(updateUserDto);
        var notFoundResult = result as NotFoundObjectResult;
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(notFoundResult?.Value, Is.EqualTo("User not found."));
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
        });


    }
}