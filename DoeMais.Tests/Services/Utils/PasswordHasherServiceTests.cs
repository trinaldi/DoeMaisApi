using DoeMais.Services.Interfaces.Utils;
using DoeMais.Services.Utils;

namespace DoeMais.Tests.Services.Utils;

[TestFixture]
public class PasswordHasherServiceTests
{
    private IPasswordHasher _passwordHasherService = null!;

    [SetUp]
    public void Setup()
    {
        _passwordHasherService = new PasswordHasherService();
    }

    [Test]
    public void HashPassword_ShouldGenerateHashedPassword()
    {
        var password = "password";
        
        var hash = _passwordHasherService.HashPassword(password);
        
        Assert.That(hash, Is.Not.Null);
        Assert.That(hash, Is.Not.EqualTo(password));
        Assert.That(hash.Length, Is.GreaterThan(20));
    }

    [Test]
    public void VerifyHashedPassword_ShouldVerifyHashedPassword()
    {
        var password = "password";
        var hash = _passwordHasherService.HashPassword(password);
        
        var verified = _passwordHasherService.VerifyHashedPassword(password, hash);
        
        Assert.That(verified, Is.True);
    }
    
}