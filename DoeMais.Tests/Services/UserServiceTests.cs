using NUnit.Framework;
using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTO.User;
using DoeMais.Exceptions;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using DoeMais.Tests.Helpers;
using DoeMais.Tests.Helpers.Asserts;
using Moq;

namespace DoeMais.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;
    private User _user;
    private UserProfileDto _userProfileDto;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
        _user = FakeUser.Create().ToEntity();
        _userProfileDto = _user.ToDto();
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var comparer = new RecordDeepEqualityComparer<UserProfileDto>();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(_userProfileDto.UserId))
            .ReturnsAsync(_user);
        
        var result = await _userService.GetByIdAsync(_userProfileDto.UserId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(_userProfileDto).Using(comparer));
        });
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(r => r.GetByIdAsync(_userProfileDto.UserId))
            .ReturnsAsync((User?)null);
        
        var result = await _userService.GetByIdAsync(_userProfileDto.UserId);
        
        Assert.That(result.Data, Is.Null);
    }

    [Test]
    public async Task PutAsync_ShouldUpdateUser_WhenNameIsChanged()
    {
        var userInRepo = _user.Clone();
        var newName = Guid.NewGuid().ToString();
        var updateUserDto = new UpdateUserDto { Name = newName };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userInRepo.UserId))
            .ReturnsAsync(userInRepo);
        
        await _userService.UpdateUserAsync(userInRepo.UserId, updateUserDto);
        
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.UserId == userInRepo.UserId &&
            u.Name == updateUserDto.Name
        )), Times.Once);
    }

    [Test]
    public async Task PutAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        const long userId = 123;
        var newName = Guid.NewGuid().ToString();
        var updateUserDto = new UpdateUserDto { Name = newName };
        
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);
        var result = await _userService.UpdateUserAsync(_user.UserId, updateUserDto);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Type, Is.Not.EqualTo(ResultType.Success));
        });
    }
    
}