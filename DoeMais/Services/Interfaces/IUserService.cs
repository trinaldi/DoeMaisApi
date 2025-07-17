using DoeMais.Domain.Entities;
using DoeMais.DTO.User;

namespace DoeMais.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> UpdateUserAsync(long userId, UpdateUserDto dto);
}