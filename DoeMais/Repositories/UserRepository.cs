using DoeMais.Data;
using DoeMais.Domain.Entities;
using DoeMais.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    
    public UserRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<User?> GetByIdAsync(long id)
    {
        return await _ctx.Users.FindAsync(id);
    }

    public async Task UpdateAsync(User user)
    {
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
    }
}