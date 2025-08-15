using DoeMais.Infrastructure;
using DoeMais.Services.Query;
using Microsoft.EntityFrameworkCore;

namespace DoeMais.Tests.Helpers.Factories;

public static class TestDbContextFactory
{
    public static AppDbContext CreateInMemoryContext(ICurrentUserService currentUserService)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options, currentUserService);
        return context;

    } 
}