namespace DoeMais.Services.Interfaces.Utils;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}