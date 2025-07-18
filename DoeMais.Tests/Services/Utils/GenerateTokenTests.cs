using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoeMais.Domain.Entities;
using DoeMais.Services.Interfaces.Utils;
using DoeMais.Services.Utils;
using DoeMais.Tests.Domain;
using DoeMais.Tests.Extensions;
using Microsoft.Extensions.Configuration;

namespace DoeMais.Tests.Services.Utils;

[TestFixture]
public class GenerateTokenTests
{
    private ITokenGenerator _tokenGeneratorService;
    private string _jwtKey;
    private string _issuer;
    private User _user;

    [SetUp]
    public void SetUp()
    {
        _jwtKey = Guid.NewGuid().ToString();
        _issuer = "DoeMaisApiTests";
        _user = FakeUser.Create().ToUser();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", _jwtKey },
            { "Jwt:Issuer", _issuer },
        };
        
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        _tokenGeneratorService = new TokenGeneratorService(config);
    }

    [Test]
    public void GenerateToken_GeneratesCorrectToken()
    {
        var token = _tokenGeneratorService.GenerateToken(_user);
        
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.WhiteSpace);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        Assert.Multiple(() =>
        {
            Assert.That(jwt.Claims.Any(c => c.Type == "sub" && c.Value == _user.UserId.ToString()));
            Assert.That(jwt.Claims.Any(c => c.Type == "email" && c.Value == _user.Email));
            Assert.That(jwt.Claims.Any(c =>
                c.Type == ClaimTypes.Role && 
                c.Value == _user.UserRoles.First().Role.Name
            ), Is.True);
            Assert.That(jwt.Issuer, Is.EqualTo(_issuer));
            Assert.That(jwt.ValidTo, Is.GreaterThan(DateTime.UtcNow));
        });
    }

}