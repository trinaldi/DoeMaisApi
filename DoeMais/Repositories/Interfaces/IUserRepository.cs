using DoeMais.Domain.Entities;

namespace DoeMais.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task UpdateAsync(User user); 
}