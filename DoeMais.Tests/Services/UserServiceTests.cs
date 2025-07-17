using NUnit.Framework;
using DoeMais.Domain.Entities;
using DoeMais.Exceptions;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using Moq;

namespace DoeMais.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = FakeUser.CreateFakeUser().ToUser();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.UserId))
            .ReturnsAsync(user);
        
        var result = await _userService.GetByIdAsync(user.UserId);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(user));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var user = FakeUser.CreateFakeUser().ToUser();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.UserId))
            .ReturnsAsync((User?)null);
        
        var result = await _userService.GetByIdAsync(user.UserId);
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task PutAsync_ShouldUpdateUser_WhenNameIsChanged()
    {
        var user = FakeUser.CreateFakeUser().ToUser();
        var newName = Guid.NewGuid().ToString();
        user.Name = newName;
        var updateUserDto = user.ToUpdateUserDto();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.UserId))
            .ReturnsAsync(user);
        
        var result = await _userService.UpdateUserAsync(user.UserId, updateUserDto);
        
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.UserId == user.UserId &&
            u.Name == updateUserDto.Name
        )), Times.Once);
    }

    [Test]
    public void PutAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var user = FakeUser.CreateFakeUser().ToUser();
        var newName = Guid.NewGuid().ToString();
        user.Name = newName;
        var updateUserDto = user.ToUpdateUserDto();
        
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.UserId))
            .ReturnsAsync((User?)null);
        
        var ex = Assert.ThrowsAsync<NotFoundException<User>>(() => _userService.UpdateUserAsync(user.UserId, updateUserDto));
        Assert.That(ex!.Message, Is.EqualTo("User not found."));
            
    }
    
}