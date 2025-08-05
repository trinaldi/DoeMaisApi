using DoeMais.Common;
using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTO.User;
using DoeMais.Extensions;
using DoeMais.Repositories.Interfaces;
using DoeMais.Services.Interfaces;

namespace DoeMais.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserProfileDto?>> GetByIdAsync(long id)
    { 
        var result = await _userRepository.GetByIdAsync(id);
        return result != null 
            ? new Result<UserProfileDto?>(ResultType.Success, result.ToDto())
            : new Result<UserProfileDto?>(ResultType.NotFound, result?.ToDto(), "User not found.");
    }
    public async Task<Result<UserProfileDto?>> UpdateUserAsync(long userId, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return new Result<UserProfileDto?>(ResultType.NotFound, null, "User not found.");

        user.UpdateFromDto(dto);
        await _userRepository.UpdateAsync(user);

        return new Result<UserProfileDto?>(ResultType.Success, user.ToDto());
    }


}