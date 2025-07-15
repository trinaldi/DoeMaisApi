using System.IdentityModel.Tokens.Jwt;
using DoeMais.Models;
using DoeMais.Services.Interfaces.Utils;
using DoeMais.Services.Utils;
using Microsoft.Extensions.Configuration;

namespace DoeMais.Tests.Services.Utils;

[TestFixture]
public class GenerateTokenTests
{
    private ITokenGenerator _tokenGeneratorService;
    private string _jwtKey;
    private string _issuer;
    private Random _random;

    [SetUp]
    public void SetUp()
    {
        _random = new Random();
        _jwtKey = Guid.NewGuid().ToString();
        _issuer = "DoeMaisApiTests";

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
        var user = new User
        {
            UserId = _random.NextInt64(),
            Email = "test@email.com",
            Role = "Donor"
        };
        
        var token = _tokenGeneratorService.GenerateToken(user);
        
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.WhiteSpace);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        
        Assert.That(jwt.Claims.Any(c => c.Type == "sub" && c.Value == user.UserId.ToString()));
        Assert.That(jwt.Claims.Any(c => c.Type == "email" && c.Value == user.Email));
        Assert.That(jwt.Claims.Any(c => c.Type == "role" && c.Value == user.Role));
        Assert.That(jwt.Issuer, Is.EqualTo(_issuer));
        Assert.That(jwt.ValidTo, Is.GreaterThan(DateTime.UtcNow));
    }

}