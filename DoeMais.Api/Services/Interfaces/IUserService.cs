using DoeMais.Common;
using DoeMais.DTOs.User;

namespace DoeMais.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserProfileDto?>> GetByIdAsync(long id);
    Task<Result<UserProfileDto?>> UpdateUserAsync(long userId, UpdateUserDto dto);
}